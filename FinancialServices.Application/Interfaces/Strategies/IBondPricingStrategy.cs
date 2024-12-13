using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.RequestObjects;


namespace PublicBonds.Application.Interfaces.Strategies
{
    public interface IBondPricingStrategy
    {
        Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request);
    }
}
