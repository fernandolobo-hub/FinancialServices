using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Interfaces.Repositories
{
    public interface IBondRepository : IGenericRepository<Bond>
    {
        Task<int> SaveAsync(Bond bond);

        Task<IEnumerable<Bond>> GetBondsAsync(int bondTypeId);
    }
}
