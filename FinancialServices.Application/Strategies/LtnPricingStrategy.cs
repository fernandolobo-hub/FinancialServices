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
            var bondPaymentDate = await _chronosService.GetNextBusinessDayAsync(request.BondMaturityDate);
            var businessDays = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, bondPaymentDate);
            var annualRate = request.Rate; 
            decimal years = businessDays / 252.0m;
            var discountFactor = (decimal)Math.Pow(1 + (double)annualRate, (double)years);

            decimal faceValue = 1000.0m;
            decimal presentValue = faceValue / discountFactor;

            var cashFlow = new CashFlowPayment
            {
                Date = bondPaymentDate,
                BusinessDays = businessDays,
                PresentValue = Math.Round(presentValue * 100) / 100,
                FutureValue = faceValue,
                Coupon = false
            };

            decimal duration = years;

            return new BondPricingResponse
            {
                CashFlowPayments = new List<CashFlowPayment> { cashFlow },
                Duration = duration,
                PresentValue = Math.Truncate(presentValue * 100) / 100
            };
        }
    }
}
