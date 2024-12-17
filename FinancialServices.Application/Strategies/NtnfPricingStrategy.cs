using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Strategies
{
    public class NtnfPricingStrategy : IBondPricingStrategy
    {
        private readonly IChronosService _chronosService;

        private const decimal AnnualCoupon = 0.1m;
        private const decimal FaceValue = 1000.00m;

        private static readonly (int Month, int Day)[] SemiannualPaymentDates =
        {
            (1, 1),
            (7, 1)
        };

        public NtnfPricingStrategy(IChronosService chronosService)
        {
            _chronosService = chronosService;
        }

        public async Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var bond = BondCaching.GetBondByNameAndMaturityDate(request.BondName, request.BondMaturityDate);

            var cashFlows = await GenerateCashFlowsAsync(bond, request);

            decimal sumPV = 0.0m;
            decimal weightedSum = 0.0m;
            decimal rate = request.Rate;

            foreach (var cf in cashFlows)
            {
                decimal years = cf.BusinessDays / 252.0m;
                decimal discountFactor = (decimal)Math.Pow((double)(1 + rate), (double)years);

                cf.PresentValue = cf.FutureValue / discountFactor;

                sumPV += cf.PresentValue;
                weightedSum += years * cf.PresentValue;
            }

            decimal duration = sumPV > 0 ? weightedSum / sumPV : 0.0m;

            return new BondPricingResponse
            {
                CashFlowPayments = cashFlows,
                Duration = duration,
                PresentValue = Math.Truncate(100 * sumPV) / 100
            };
        }

        private async Task<List<CashFlowPayment>> GenerateCashFlowsAsync(Bond bond, BondPricingRequest request)
        {
            var cashFlows = new List<CashFlowPayment>();

            DateTime couponDate = FindNextCouponDate(request.PurchaseDate, bond.MaturityDate);

            couponDate = await _chronosService.GetNextBusinessDayAsync(couponDate);

            var bondFinalPaymentDate = await _chronosService.GetNextBusinessDayAsync(bond.MaturityDate);

            while (couponDate <= bondFinalPaymentDate)
            {
                int businessDaysToCoupon = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, couponDate);
                decimal couponFutureValue = ((decimal)Math.Pow(1 + (double)AnnualCoupon , 0.5) - 1) * FaceValue;
                couponFutureValue = Math.Round(couponFutureValue, 2);

                cashFlows.Add(new CashFlowPayment
                {
                    Date = couponDate,
                    BusinessDays = businessDaysToCoupon,
                    FutureValue = couponFutureValue,
                    PresentValue = 0,
                    Coupon = true
                });

                couponDate = GetNextSemiannualDate(couponDate);
                couponDate = await _chronosService.GetNextBusinessDayAsync(couponDate);
            }

            // Se a data de cupom for exatamente o vencimento, geramos DOIS fluxos (cupom + principal)
            // ou se cair no loop final, apenas principal.
            if (couponDate == bond.MaturityDate)
            {
                // 1) Cupom final
                int businessDaysToCoupon = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, couponDate);
                decimal couponFutureValue = (decimal)(Math.Pow(1 + (double)AnnualCoupon, 0.5) - 1) * FaceValue;

                cashFlows.Add(new CashFlowPayment
                {
                    Date = couponDate,
                    BusinessDays = businessDaysToCoupon,
                    FutureValue = Math.Truncate(100 * couponFutureValue) / 100,
                    PresentValue = 0,
                    Coupon = true
                });
            }

            DateTime maturityBusinessDay = await _chronosService.GetNextBusinessDayAsync(bond.MaturityDate);
            int businessDaysToMaturity = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, maturityBusinessDay);
            cashFlows.Add(new CashFlowPayment
            {
                Date = maturityBusinessDay,
                BusinessDays = businessDaysToMaturity,
                FutureValue = FaceValue,
                PresentValue = 0.0m,
                Coupon = false
            });

            return cashFlows;
        }


        private DateTime FindNextCouponDate(DateTime referenceDate, DateTime maturityDate)
        {
            DateTime candidate = new DateTime(referenceDate.Year, SemiannualPaymentDates[0].Month, SemiannualPaymentDates[0].Day);
            if (candidate <= referenceDate.Date)
            {
                candidate = new DateTime(referenceDate.Year, SemiannualPaymentDates[1].Month, SemiannualPaymentDates[1].Day);
                if (candidate <= referenceDate.Date)
                {
                    candidate = new DateTime(referenceDate.Year + 1, SemiannualPaymentDates[0].Month, SemiannualPaymentDates[0].Day);
                }
            }

            while (candidate <= referenceDate.Date)
            {
                candidate = GetNextSemiannualDate(candidate);
            }

            if (candidate > maturityDate)
                candidate = maturityDate;

            return candidate;
        }

        private DateTime GetNextSemiannualDate(DateTime currentCouponDate)
        {
            if (currentCouponDate.Month == 1)
                return new DateTime(currentCouponDate.Year, 7, 1);
            else
                return new DateTime(currentCouponDate.Year + 1, 1, 1);
        }
    }
}
