using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Interfaces.Services
{
    public interface IDailyPricesService
    {
        Task ImportAllHistoricalDailyBondsData(PublicBondHistoricalImportFilterRequest request);
        Task<IEnumerable<DailyBondInfoDto>> GetHistoricalPrices(PublicBondsHistoricalDataFilterRequest request);
    }

}
