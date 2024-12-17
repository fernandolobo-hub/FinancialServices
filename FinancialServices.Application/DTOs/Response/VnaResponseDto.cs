using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class VnaResponseDto
    {
        public string Index { get; set; }

        public decimal NominalValue { get; set; }

        public DateTime ReferenceDate { get; set; }

        public static VnaResponseDto FromVna(Vna vna)
        {
            return new VnaResponseDto
            {
                Index = vna.Indexer.ToString(),
                NominalValue = vna.NominalValue,
                ReferenceDate = vna.ReferenceDate
            };
        }
    }
}
