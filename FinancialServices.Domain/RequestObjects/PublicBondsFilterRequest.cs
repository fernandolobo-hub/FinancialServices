using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.RequestObjects
{
    public class PublicBondsFilterRequest
    {
        public bool? HasCoupon { get; set; }
        public string? RateType { get; set; }
        public bool? IsBeingOfferedOnPrimaryMarket { get; set; }
    }
}
