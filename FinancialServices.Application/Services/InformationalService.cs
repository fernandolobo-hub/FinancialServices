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

        public Task<IEnumerable<Bond>> GetAvailableBonds()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BondType>> GetAvailableBondTypes()
        {
            try
            {
                var bonds = _publicBondTypesRepository.GetAllAsync();
                return bonds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
