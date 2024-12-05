using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.Entities;

namespace PublicBonds.Application.Interfaces.Repositories
{
    public interface IDailyBondPricesRepository
    {
        Task<int> Import(IList<DailyBondInfo> dailyBondInfos);

        Task<bool> HasBondBeenImported(int bondId, int year);

        Task<bool> DeleteByBondId(int bondId, int year);

        Task<IEnumerable<DailyBondInfoDto>> GetByBondId(int bondId, int startYear, int endYear);
    }
}
