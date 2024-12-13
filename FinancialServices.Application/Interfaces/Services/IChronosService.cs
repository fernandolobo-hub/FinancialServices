namespace PublicBonds.Application.Interfaces.Services
{
    public interface IChronosService
    {
        Task<int> GetBusinessDaysAsync(DateTime referenceDate, DateTime endDate);
    }
}
