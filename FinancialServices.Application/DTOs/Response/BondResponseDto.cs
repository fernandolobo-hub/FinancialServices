using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class BondResponseDto
    {
        public required string BondName { get; set; }
        public DateTime MaturityDate { get; set; }
        public required BondTypeResponseDto BondType { get; set; }

        public static BondResponseDto FromBond(Bond bond)
        {
            return new BondResponseDto
            {
                BondName = bond.Name,
                MaturityDate = bond.MaturityDate,
                BondType = BondTypeResponseDto.FromBondType(bond.Type)
            };
        }
    }
}
