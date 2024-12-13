using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;


namespace PublicBonds.Application.Services
{
    public class HolidaysService : IHolidaysService
    {

        private readonly IHolidaysRepository _holidaysRepository;

        public HolidaysService(IHolidaysRepository holidaysRepository) 
        {
            _holidaysRepository = holidaysRepository;
        }

        public async Task<HashSet<DateTime>> GetHollidaysAsync(DateTime startDate, DateTime endDate)
        {
            var holidays = await _holidaysRepository.GetHollidaysAsync(startDate, endDate);

            return holidays;
        }
    }
}
