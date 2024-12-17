namespace PublicBonds.Domain.RequestObjects
{
    public class BondPricingRequest
    {
        public required string BondName { get; set; }

        public required DateTime BondMaturityDate { get; set; }

        public required DateTime PurchaseDate { get; set; }

        public required decimal Rate { get; set; }

        public required decimal Quantity { get; set; }
    }
}
