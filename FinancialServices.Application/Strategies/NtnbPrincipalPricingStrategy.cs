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
        private const decimal IpcaProjected = 0.0059m;

        public NtnbPrincipalStrategy(IChronosService chronosService, IVnaService vnaService)
        {
            _chronosService = chronosService;
            _vnaService = vnaService;
        }

        public async Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var bond = BondCaching.GetBondByNameAndMaturityDate(request.BondName, request.BondMaturityDate);

            // 1) Obter VNA base
            var vnaDto = await _vnaService.GetMostRecentVnaAsync(request.PurchaseDate, IndexerEnum.Ipca);
            if (vnaDto == null)
                throw new InvalidOperationException("Nenhum VNA encontrado para IPCA nessa data de referência.");

            decimal vnaBase = vnaDto.NominalValue;
            var liquidationDate = await _chronosService.GetNextBusinessDayAsync(request.PurchaseDate.AddDays(1));

            // 2) Calcular dias úteis fracionados para projeção (dia 15)
            DateTime midMonthCurrent = _chronosService.GetMidMonthDate(request.PurchaseDate.Date);      // Dia 15 do mês atual ou passado
            DateTime midMonthNext = _chronosService.GetNextMidMonthDate(midMonthCurrent);   // Dia 15 do mês seguinte

            // Ex.: diasUteis1 = entre data de compra e midMonthCurrent ou vice-versa
            int businessDaysNumerator = _chronosService.GetDaysBetweenVna15AndDate(liquidationDate, vnaDto.ReferenceDate);

            // diasUteis2 = entre midMonthCurrent e midMonthNext
            int businessDaysDenominator = DateTime.DaysInMonth(vnaDto.ReferenceDate.Year, vnaDto.ReferenceDate.Month);

            // 3) Projeção do VNA => vnaBase * (1 + IPCAproj)^(diasUteis1 / diasUteis2)
            decimal expo = (decimal)businessDaysNumerator / businessDaysDenominator;
            decimal factor = (decimal)Math.Pow((double)(1 + IpcaProjected), (double)expo);
            decimal vnaProjected = vnaBase * factor;

            // 4) Calcular dias úteis até o vencimento e discountFactor
            int businessDays = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, bond.MaturityDate);
            decimal years = businessDays / 252.0m;

            // discountFactor = 100 / (1 + rate)^(DU/252)
            decimal discountFactor = 100.0m / (decimal)Math.Pow((double)(1 + request.Rate), (double)years);

            // 5) Price = VNAproj * (discountFactor / 100)
            decimal price = vnaProjected * (discountFactor / 100.0m);

            // Montar CashFlowPayment
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
