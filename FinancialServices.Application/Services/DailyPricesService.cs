using ExcelDataReader;
using PublicBonds.Domain.Exceptions;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Persistance;
using PublicBonds.Application.Validators;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using PublicBonds.Domain.Exceptions.Request;


namespace PublicBonds.Application.Services
{
    public class DailyPricesService : IDailyPricesService
    {
        private readonly IBondRepository _bondRepository;
        private readonly IDailyBondPricesRepository _dailyBondPricesRepository;
        private readonly IValidator<PublicBondHistoricalImportFilterRequest> _publicBondHistoricalImportFilterValidator;
        private readonly string? _tesouroDiretoUrl;

        public DailyPricesService(IInformationalService publicBondsInfoService,
            IValidator<PublicBondHistoricalImportFilterRequest> publicBondHistoricalImportFilterValidator,
            IBondRepository bondRepository,
            IDailyBondPricesRepository dailyBondsImportRepository,
            IConfiguration configuration)
        {
            _publicBondHistoricalImportFilterValidator = publicBondHistoricalImportFilterValidator;
            _bondRepository = bondRepository;
            _dailyBondPricesRepository = dailyBondsImportRepository;
            _tesouroDiretoUrl = configuration.GetSection("TesouroDireto").GetSection("BaseUrl").Value;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task ImportAllHistoricalDailyBondsData(PublicBondHistoricalImportFilterRequest request)
        {
            try
            {
                var validationResult = await _publicBondHistoricalImportFilterValidator.ValidateAsync(request);
                if (validationResult == null || !validationResult.IsValid)
                {
                    var errors = validationResult?.Errors
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                        .ToList() ?? new List<string> { "O resultado da validação está vazio." };

                    throw new DailyBondImportRequestValidationException("Request Validation Error", errors);
                }

                if (request.Year is null)
                {
                    await ProcessAllYearsAsync(request.BondName);
                }
                else
                {
                    await ProcessSingleYearAsync(request.BondName, (int)request.Year);
                }
            }
            catch(Exception)
            {
                Console.WriteLine($"Erro na importacao do titulo {request.ToString()}");
            }
            
        }

        private async Task ProcessAllYearsAsync(string bondTypeName)
        {
            try
            {
                var bondType = BondTypeCaching.GetBondTypeByName(bondTypeName);
                var currentYear = bondType.FirstTradedAt;

                while (currentYear <= DateTime.Now.Year)
                {
                    Console.Write($"Importando dados do titulo {bondTypeName} disponiveis no ano {currentYear}");
                    await ProcessSingleYearAsync(bondTypeName, currentYear);
                    currentYear++;
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            
        }

        private async Task ProcessSingleYearAsync(string bondTypeName, int year)
        {
            string filePath = GenerateFilePath(bondTypeName, year);
            var stream = await DownloadFileAsync(_tesouroDiretoUrl + filePath);

            var dailyBondInfos = await ReadExcelFile(stream, BondTypeCaching.GetBondTypeByName(bondTypeName));

            var ids = dailyBondInfos
                    .Select(d => d.Bond.Id) 
                    .Distinct()             
                    .ToList();

            foreach(var id in ids)
            {
                var bondBatch = dailyBondInfos.Where(dailyBondInfos => dailyBondInfos.Bond.Id == id).ToList();
                var bondHasBeenImported = await HasBondDailyInfoBeenImported(id, year);
                if(bondHasBeenImported)
                {
                    await _dailyBondPricesRepository.DeleteByBondId(id, year);
                }
                await _dailyBondPricesRepository.Import(bondBatch);
            }
        }
        private async Task<bool> HasBondDailyInfoBeenImported(int bondId, int year)
        {
            return await _dailyBondPricesRepository.HasBondBeenImported(bondId, year);

        }

        //tratar exceçoes
        private static async Task<Stream> DownloadFileAsync(string downloadPath)
        {
            using HttpClient client = new();
            try
            {
                var response = await client.GetAsync(downloadPath);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"Download do arquivo {downloadPath} não disponivel no tesouro direto");
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        private async Task<List<DailyBondInfo>> ReadExcelFile(Stream fileStream, BondType bondType)
        {
            var dailyBondInfos = new List<DailyBondInfo>();


            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                do
                {
                    var currentSheetName = reader.Name;
                    var maturityDateText = Regex.Match(currentSheetName, @"\d+").Value;
                    var bondMaturityDate = TypeConverter.ToDateTime(maturityDateText);
                    var bond = new Bond(bondType, bondMaturityDate);
                    Console.WriteLine($"Iniciando leitura do sheet referente ao titulo {bond.Name}");
                    bond.Id = await SaveBondsIfNotRegisteredAsync(bond);
                    {
                        var rowIndex = 0;
                        while (reader.Read())
                        {
                            if (rowIndex++ < 2) continue;

                            var dailyBondInfo = new DailyBondInfo
                            {
                                Bond = bond,
                                Date = TypeConverter.ToDateTime(reader.GetValue(0)),
                                MorningBuyRate = TypeConverter.ToPercentageDecimal(reader.GetValue(1)),
                                MorningSellRate = TypeConverter.ToPercentageDecimal(reader.GetValue(2)),
                                MorningBuyPrice = TypeConverter.ToDecimal(reader.GetValue(3)),
                                MorningSellPrice = TypeConverter.ToDecimal(reader.GetValue(4))
                            };

                            dailyBondInfos.Add(dailyBondInfo);
                        }
                    }
                }
                while (reader.NextResult());

            }
            return dailyBondInfos;
        }

        public static string GenerateFilePath(string bondName, int year)
        {
            var bond = BondTypeCaching.GetBondTypeByName(bondName);
            var sigla = bond.Category.Replace(" ", "_");
            return $"{year}/{sigla}_{year}.xls";
        }

        private async Task<int> SaveBondsIfNotRegisteredAsync(Bond bond)
        {
            try
            {
                var allBonds = _bondRepository.GetAllAsync().Result.ToList();
                if (!allBonds.Any(b => b.Equals(bond)))
                {
                    Console.WriteLine($"Titulo {bond.Name} sem registros na base de dados. Iniciando gravacao");
                    return await _bondRepository.SaveAsync(bond);
                }
                var existingBond = allBonds.FirstOrDefault(b => b.Equals(bond));
                Console.WriteLine($"Titulo {bond.Name} ja esta gravado na base");
                return existingBond!.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving Unregistered bonds", ex);
                throw;
            }
        }
    }
}

