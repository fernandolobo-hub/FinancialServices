using PublicBonds.Domain.Entities;

namespace PublicBonds.Application.Interfaces.Services
{
    public interface IHolidaysService
    {
        Task<HashSet<DateTime>> GetHollidaysAsync(DateTime startDate, DateTime endDate);
    }
}
