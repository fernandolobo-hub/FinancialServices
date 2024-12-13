using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Strategies
{
    public class LtnPricingStrategy : IBondPricingStrategy
    {
        private readonly IChronosService _chronosService;

        public LtnPricingStrategy(IChronosService chronosService)
        {
            _chronosService = chronosService;
        }

        public async Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var businessDays = await _chronosService.GetBusinessDaysAsync(request.ReferenceDate, request.BondMaturityDate);
            var annualRate = request.Rate; 
            var years = businessDays / 252.0;
            var discountFactor = Math.Pow(1 + annualRate, years);

            double faceValue = 1000.0;
            double presentValue = faceValue / discountFactor;

            var cashFlow = new CashFlowPayment
            {
                Date = request.BondMaturityDate,
                BusinessDays = businessDays,
                PresentValue = presentValue,
                FutureValue = faceValue,
                Coupon = false
            };

            double duration = years;

            return new BondPricingResponse
            {
                CashFlowPayments = new List<CashFlowPayment> { cashFlow },
                Duration = duration,
                PresentValue = presentValue
            };
        }
    }
}
