using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Domain.ResponseObjects.Temps
{
    public class TemporaryPublicBondsInfoResponse
    {
        public string BondName { get; set; }
        public DateTime MaturityDate { get; set; }
        public string RateType { get; set; }
        public bool IsBeingOfferedOnPrimaryMarket { get; set; }
    }
}
