using FluentValidation;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Exceptions.Request;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Services
{
    public class InformationalService : IInformationalService
    {

        private readonly IBondTypeRepository _bondTypeRepository;
        private readonly IBondRepository _bondsRepository;
        private readonly IValidator<BondFilterRequest> _bondFilterRequestValidator;

        public InformationalService(IBondTypeRepository publicBondsInfoRepository,
            IBondRepository bondsRepository,
            IValidator<BondFilterRequest> bondFilterRequestValidator)
        {
            _bondTypeRepository = publicBondsInfoRepository;
            _bondsRepository = bondsRepository;
            _bondFilterRequestValidator = bondFilterRequestValidator;
        }

        public async Task<IEnumerable<BondTypeResponseDto>> GetAvailableBondTypes()
        {
            List<BondTypeResponseDto> bondTypeDtoList = [];
            try
            {
                var bondTypes = await _bondTypeRepository.GetAllAsync();
                foreach(var bond in bondTypes)
                {
                    var bondTypeDto = BondTypeResponseDto.FromBondType(bond);
                    bondTypeDtoList.Add(bondTypeDto);
                }
                return bondTypeDtoList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BondResponseDto>> GetBondsAsync(BondFilterRequest request)
        {
            List<BondResponseDto> bondResponseDtoList = [];
            try
            {
                var validationResult = await _bondFilterRequestValidator.ValidateAsync(request);
                if (validationResult == null || !validationResult.IsValid)
                {
                    var errors = validationResult?.Errors
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                        .ToList() ?? new List<string> { "O resultado da validação está vazio." };

                    throw new BondRequestValidationException($"Invalid Request. {String.Join(",", errors)}", errors);
                }

                var bondType = BondTypeCaching.GetBondTypeByName(request.BondTypeName);

                var bonds = await _bondsRepository.GetBondsAsync(bondType.Id);

                if (!request.IncludeMaturedBonds)
                {
                    bonds = bonds.Where(b => b.MaturityDate > DateTime.Now);
                }
                
                foreach(var bond in bonds)
                {
                    bondResponseDtoList.Add(BondResponseDto.FromBond(bond));
                }

                return bondResponseDtoList;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao obter os Bonds da base", ex);
                throw;
            }
        }
    }
}
