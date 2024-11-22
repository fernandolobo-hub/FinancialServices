using FinancialServices.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialServices.Domain.Entities
{
    public class BondType : Entity
    {
        public required string Name { get; set; }
        public bool HasCoupon { get; set; }
        public double? AnnualCouponPercentage { get; set; }
        public IndexerEnum? Indexer { get; set; }
        public DateTime? VnaDateBase { get; set; }
        public RateType RateType { get; set; }
        public required string Category { get; set; }
        public int FirstTradedAt { get; set; }
    }
}
