using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Services
{
    public class InformationalService : IInformationalService
    {

        private readonly IBondTypeRepository _publicBondTypesRepository;

        public InformationalService(IBondTypeRepository publicBondsInfoRepository)
        {
            _publicBondTypesRepository = publicBondsInfoRepository;
        }

        public async Task<IEnumerable<BondTypeResponseDto>> GetAvailableBondTypes()
        {
            List<BondTypeResponseDto> bondTypeDtoList = [];
            try
            {
                var bondTypes = await _publicBondTypesRepository.GetAllAsync();
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

        public Task<IEnumerable<Bond>> GetAvailableBonds(BondFilterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
