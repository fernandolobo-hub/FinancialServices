using PublicBonds.Domain.Entities;

namespace PublicBonds.Application.Interfaces.Repositories
{
    public interface IHolidaysRepository
    {
        Task<HashSet<DateTime>> GetHollidaysAsync(DateTime startDate, DateTime endDate);
    }
}
