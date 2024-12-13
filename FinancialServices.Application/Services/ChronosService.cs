using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;


namespace PublicBonds.Application.Services
{
    public class ChronosService : IChronosService
    {
        private readonly IHolidaysService _holidaysService;

        public ChronosService(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        public async Task<int> GetBusinessDaysAsync(DateTime referenceDate, DateTime endDate)
        {
            var holidays = await _holidaysService.GetHollidaysAsync(referenceDate, endDate);

            int businessDays = 0;
            DateTime currentDate = referenceDate;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(currentDate.Date))
                {
                    businessDays++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return businessDays - 1;
        }
    }
}
