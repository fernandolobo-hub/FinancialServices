namespace PublicBonds.Domain.RequestObjects
{
    public record PublicBondHistoricalImportFilterRequest
    {
        public required string BondName { get; set; }
        public int? Year { get; set; }
    }
}
