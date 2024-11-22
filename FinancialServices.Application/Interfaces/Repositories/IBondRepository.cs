using FinancialServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Application.Interfaces.Repositories
{
    public interface IBondRepository : IGenericRepository<Bond>
    {
        Task<int> SaveAsync(Bond bond);
    }
}
