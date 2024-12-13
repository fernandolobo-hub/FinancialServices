namespace PublicBonds.Domain.RequestObjects
{
    public class BondFilterRequest
    {
        public required string BondTypeName { get; set; }
        public bool IncludeMaturedBonds { get; set; } = true;
    }
}
