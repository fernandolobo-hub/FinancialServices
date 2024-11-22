using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Application.Interfaces.Services;
using FinancialServices.Domain.Entities;
using FinancialServices.Domain.RequestObjects;

namespace FinancialServices.Application.Services
{
    public class PublicBondTypesService : IPublicBondsInfoService
    {

        private readonly IBondTypeRepository _publicBondTypesRepository;

        public PublicBondTypesService(IBondTypeRepository publicBondsInfoRepository)
        {
            _publicBondTypesRepository = publicBondsInfoRepository;
        }

        public Task<IEnumerable<BondType>> GetAllAvailableBondTypes()
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
