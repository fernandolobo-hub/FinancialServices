using FinancialServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Application.Interfaces.Repositories
{
    public interface IDailyBondsImportRepository
    {
        Task<int> ImportDailyBonds(IList<DailyBondInfo> dailyBondInfos);

        Task<bool> HasBondBeenImported(int bondId, int year);

        Task<bool> DeleteByBondId(int bondId, int year);
    }
}
