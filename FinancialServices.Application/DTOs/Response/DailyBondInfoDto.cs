using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class DailyBondInfoDto
    {
        public DateTime Date { get; set; }

        public decimal MorningBuyRate { get; set; }

        public decimal MorningSellRate { get; set; }

        public decimal MorningBuyPrice { get; set; }

        public decimal MorningSellPrice { get; set; }
    }
}
