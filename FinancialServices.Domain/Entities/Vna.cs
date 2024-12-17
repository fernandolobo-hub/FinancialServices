using PublicBonds.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Entities
{
    public class Vna : Entity
    {
        public IndexerEnum Indexer { get; set; }

        public decimal NominalValue { get; set; }

        public DateTime ReferenceDate { get; set; }
    }
}
