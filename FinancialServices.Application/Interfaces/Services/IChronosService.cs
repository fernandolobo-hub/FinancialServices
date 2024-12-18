namespace PublicBonds.Application.Interfaces.Services
{
    public interface IChronosService
    {
        Task<int> GetBusinessDaysAsync(DateTime referenceDate, DateTime endDate);

        Task<DateTime> GetNextBusinessDayAsync(DateTime referenceDate);

        int GetDaysBetweenVna15AndDate(DateTime referenceDate, DateTime vnaDate);

        DateTime GetMidMonthDate(DateTime referenceDate);

        DateTime GetNextMidMonthDate(DateTime midMonthCurrent);
    }
}
