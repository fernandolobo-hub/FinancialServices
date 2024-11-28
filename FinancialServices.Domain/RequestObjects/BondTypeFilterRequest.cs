using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.RequestObjects
{
    public class BondTypeFilterRequest
    {
        public bool? HasCoupon { get; set; }
    }
}
