using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class BondTypeResponseDto
    {
        public required string BondTypeName {  get; set; }
        public required string Category { get; set; }
        public double? AnnualCouponPercentage { get; set; }
        public string RateType { get; set; }
        public string? Indexer { get; set; }

        public static BondTypeResponseDto FromBondType(BondType bondType)
        {
            return new BondTypeResponseDto {
                RateType = bondType.RateType.ToString(),
                BondTypeName = bondType.Name,
                Category = bondType.Category,
                AnnualCouponPercentage = bondType.AnnualCouponPercentage,
                Indexer = bondType.Indexer.ToString()
            };
        }
    }
}
