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

        private const double AnnualCoupon = 0.10;
        private const double FaceValue = 1000.0;
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

            double sumPV = 0.0;
            double weightedSum = 0.0;
            double rate = request.Rate;

            foreach (var cf in cashFlows)
            {
                double years = cf.BusinessDays / 252.0;
                double discountFactor = Math.Pow(1 + rate, years);

                cf.PresentValue = cf.FutureValue / discountFactor;

                sumPV += cf.PresentValue;
                weightedSum += years * cf.PresentValue;
            }

            // 3) Duration (Macaulay)
            double duration = sumPV > 0 ? weightedSum / sumPV : 0.0;

            // 4) Monta o resultado
            return new BondPricingResponse
            {
                CashFlowPayments = cashFlows,
                Duration = duration,
                PresentValue = sumPV
            };
        }

        /// <summary>
        /// Gera os fluxos de cupom semestrais e o principal no vencimento, populando FutureValue e PresentValue.
        /// PresentValue será calculado posteriormente no loop principal.
        /// </summary>
        private async Task<List<CashFlowPayment>> GenerateCashFlowsAsync(Bond bond, BondPricingRequest request)
        {
            var cashFlows = new List<CashFlowPayment>();

            // Localiza a primeira data de cupom semestral após a data de referência
            DateTime currentCouponDate = FindNextCouponDate(request.ReferenceDate, bond.MaturityDate);

            while (currentCouponDate <= bond.MaturityDate)
            {
                int businessDaysToCoupon = await _chronosService.GetBusinessDaysAsync(request.ReferenceDate, currentCouponDate);

                // Valor futuro do cupom semestral: ((1 + AnnualCoupon)^(0.5) - 1) * FaceValue
                double couponFutureValue = (Math.Pow(1 + AnnualCoupon, 0.5) - 1) * FaceValue;

                cashFlows.Add(new CashFlowPayment
                {
                    Date = currentCouponDate,
                    BusinessDays = businessDaysToCoupon,
                    FutureValue = couponFutureValue,  // Valor futuro do cupom
                    PresentValue = 0.0,               // Será calculado fora
                    Coupon = true
                });

                // Avança para a próxima data semestral
                currentCouponDate = GetNextSemiannualDate(currentCouponDate);
            }

            // Principal no vencimento
            int businessDaysToMaturity = await _chronosService.GetBusinessDaysAsync(request.ReferenceDate, bond.MaturityDate);
            cashFlows.Add(new CashFlowPayment
            {
                Date = bond.MaturityDate,
                BusinessDays = businessDaysToMaturity,
                FutureValue = FaceValue,     // Valor futuro (principal)
                PresentValue = 0.0,          // Será calculado fora
                Coupon = false
            });

            return cashFlows;
        }

        /// <summary>
        /// Retorna a primeira data semestral (01/01 ou 01/07) posterior à referenceDate e <= maturityDate.
        /// </summary>
        private DateTime FindNextCouponDate(DateTime referenceDate, DateTime maturityDate)
        {
            // Lógica anterior de encontrar a data semestral.
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

        /// <summary>
        /// Avança para a próxima data semestral (alternando entre 01/jan e 01/jul).
        /// </summary>
        private DateTime GetNextSemiannualDate(DateTime currentCouponDate)
        {
            if (currentCouponDate.Month == 1 && currentCouponDate.Day == 1)
                return new DateTime(currentCouponDate.Year, 7, 1);
            else
                return new DateTime(currentCouponDate.Year + 1, 1, 1);
        }
    }


}
