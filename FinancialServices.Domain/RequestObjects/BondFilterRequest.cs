using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.RequestObjects
{
    public class BondFilterRequest
    {
        string BondTypeName { get; set; } = String.Empty;
    }
}
