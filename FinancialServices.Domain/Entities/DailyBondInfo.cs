using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Domain.Entities
{
    public class DailyBondInfo : Entity
    {
        public required Bond Bond { get; set; }

        public DateTime Date { get; set; }

        public decimal MorningBuyRate { get; set; }

        public decimal MorningSellRate { get; set;}

        public decimal MorningBuyPrice { get; set; }

        public decimal MorningSellPrice { get;set; }
    }
}
