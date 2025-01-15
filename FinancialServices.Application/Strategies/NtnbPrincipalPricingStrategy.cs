using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Strategies
{
    public class NtnbPrincipalStrategy : IBondPricingStrategy
    {
        private readonly IChronosService _chronosService;
        private readonly IVnaService _vnaService;

        // Valor fixo de 0.63% (0.0063)
        private const decimal IpcaProjected = 0.0060m;

        public NtnbPrincipalStrategy(IChronosService chronosService, IVnaService vnaService)
        {
            _chronosService = chronosService;
            _vnaService = vnaService;
        }

        public async Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var bond = BondCaching.GetBondByNameAndMaturityDate(request.BondName, request.BondMaturityDate);



            var liquidationDate = await _chronosService.GetNextBusinessDayAsync(request.PurchaseDate.AddDays(1));

            var vnaDto = await _vnaService.GetMostRecentVnaAsync(liquidationDate, IndexerEnum.Ipca);
            if (vnaDto == null)
                throw new InvalidOperationException("Nenhum VNA encontrado para IPCA nessa data de referência.");

            decimal vnaBase = vnaDto.NominalValue;

            int businessDaysNumerator = _chronosService.GetDaysBetweenVna15AndDate(request.PurchaseDate, vnaDto.ReferenceDate);

            int businessDaysDenominator = DateTime.DaysInMonth(vnaDto.ReferenceDate.Year, vnaDto.ReferenceDate.Month);

            decimal expo = (decimal)businessDaysNumerator / businessDaysDenominator;
            decimal factor = (decimal)Math.Pow((double)(1 + IpcaProjected), (double)expo);
            decimal vnaProjected = vnaBase * factor;

            int businessDays = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, bond.MaturityDate);
            decimal years = businessDays / 252.0m;

            // discountFactor = 100 / (1 + rate)^(DU/252)
            decimal discountFactor = (decimal)Math.Pow((double)(1 + request.Rate), (double)years);

            // Price = VNAproj * (discountFactor / 100)
            decimal price = vnaProjected / discountFactor;

            var cashFlow = new CashFlowPayment
            {
                Date = bond.MaturityDate,
                BusinessDays = businessDays,
                PresentValue = price,
                FutureValue = vnaProjected,
                Coupon = false
            };

            decimal duration = years;

            return new BondPricingResponse
            {
                CashFlowPayments = new List<CashFlowPayment> { cashFlow },
                Duration = duration,
                PresentValue = price
            };
        }
    }
}
