using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;

namespace PublicBonds.Application.Interfaces.Repositories
{
    public interface IVnaRepositoy
    {
        Task<List<Vna>> GetVnasAsync(DateTime startDate, DateTime endDate, IndexerEnum indexer);

        Task<Vna> GetMostRecentVnaAsync(DateTime referenceDate, IndexerEnum indexer);
    }
}
