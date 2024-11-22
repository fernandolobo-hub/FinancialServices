using Dapper;
using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data.Common;

namespace FinancialServices.Infrastructure.Data.DB
{
    public class DailyBondsImportRepository : IDailyBondsImportRepository
    {

        private readonly IConfiguration _configuration;
        public DailyBondsImportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> ImportDailyBonds(IList<DailyBondInfo> dailyBondInfos)
        {
            var query = @"
                        INSERT INTO daily_bonds_info 
                            (reference_date, morning_buy_rate, morning_sell_rate, morning_buy_price, morning_sell_price, bond_id)
                        VALUES 
                            (@Date, @MorningBuyRate, @MorningSellRate, @MorningBuyPrice, @MorningSellPrice, @BondId);";

            // Mapeando os dados da lista de DailyBondInfo para o formato correto
            var parameters = dailyBondInfos.Select(d => new
            {
                Date = d.Date,
                MorningBuyRate = d.MorningBuyRate,
                MorningSellRate = d.MorningSellRate,
                MorningBuyPrice = d.MorningBuyPrice,
                MorningSellPrice = d.MorningSellPrice,
                BondId = d.Bond.Id
            });

            // Usando transação para bulk insert
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction()) 
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(query, parameters, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                
            }
        }

        public async Task<bool> HasBondBeenImported(int bondId, int year)
        {

            try
            {
                var query = $"SELECT COUNT(*)" +
                $" FROM daily_bonds_info" +
                $" WHERE bond_id = {bondId}" +
                $" AND year(reference_date) = {year};";

                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();
                    var result = await connection.QueryAsync<int>(query);
                    var recordCount = result.FirstOrDefault();
                    return recordCount > 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao verificar se o bond {bondId} foi importado para a planilha do ano {year}");
                throw;
            }

            
        }

        public async Task<bool> DeleteByBondId(int bondId, int year)
        {
            var query = @"DELETE FROM daily_bonds_info WHERE bond_id = @BondId and year(reference_date) = @Year";

            using (var connection = GetConnection())
            {
                try
                {
                    await connection.OpenAsync();
                    var rowsAffected = await connection.ExecuteAsync(query, new { BondId = bondId, Year = year });
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao deletar registros para o BondId {bondId}: {ex.Message}");
                    throw;
                }
            }
        }

    }
}
