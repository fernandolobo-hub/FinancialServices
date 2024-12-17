using PublicBonds.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.RequestObjects
{
    public class VnaFilterRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Indexer { get; set; }
    }
}
