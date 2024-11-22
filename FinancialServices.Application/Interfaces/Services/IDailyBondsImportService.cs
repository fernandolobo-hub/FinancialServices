using FinancialServices.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Application.Interfaces.Services
{
    public interface IDailyBondsImportService
    {
        Task ImportAllHistoricalDailyBondsData(PublicBondHistoricalImportFilterRequest request);
    }
}
