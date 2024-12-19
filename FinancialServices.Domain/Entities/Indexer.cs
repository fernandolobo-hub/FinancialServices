using PublicBonds.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Entities
{
    public class Indexer : Entity
    {
        public required string IndexerName { get; set; }
    }
}
