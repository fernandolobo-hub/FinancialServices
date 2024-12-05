namespace PublicBonds.Domain.RequestObjects
{
    public class PublicBondsHistoricalDataFilterRequest
    {
        public required string BondName { get; set; }

        public required DateTime MaturityDate { get; set; }

        public required int StartYear { get; set; }

        public required int EndYear { get; set; }
    }
}
