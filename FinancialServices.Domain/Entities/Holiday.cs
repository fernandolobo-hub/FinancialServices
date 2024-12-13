using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Entities
{
    public class Holiday : Entity
    {
        public required DateTime Date { get; set; }
        public string? Name { get; set; }
        public Weekday Weekday { get; set; }
    }
}
