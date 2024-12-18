using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using PublicBonds.Domain.RequestObjects;

public class NtnbPricingStrategy : IBondPricingStrategy
{
    private readonly IChronosService _chronosService;
    private readonly IVnaService _vnaService;

    // NTN-B cupom anual de 6%, pago semestralmente
    private const double AnnualCoupon = 0.06;

    // IPCA projetado (0.63%)
    private const decimal IpcaProjected = 0.0063m;

    public NtnbPricingStrategy(IChronosService chronosService, IVnaService vnaService)
    {
        _chronosService = chronosService;
        _vnaService = vnaService;
    }

    public async Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
    {
        // 1) Obter bond
        var bond = BondCaching.GetBondByNameAndMaturityDate(request.BondName, request.BondMaturityDate);

        // 2) VNA mais recente (IPCA)
        var vnaDto = await _vnaService.GetMostRecentVnaAsync(request.PurchaseDate, IndexerEnum.Ipca);
        if (vnaDto == null)
            throw new InvalidOperationException("Nenhum VNA encontrado para IPCA na data de referência.");

        decimal vnaBase = vnaDto.NominalValue;

        // 3) Calcular o VNA projetado
        // a) determina data de liquidação
        var liquidationDate = await _chronosService.GetNextBusinessDayAsync(request.PurchaseDate.AddDays(1));

        // b) dias úteis fracionados
        //    businessDaysNumerator = # dias entre 'liquidationDate' e dia 15 do VNA encontrado
        int businessDaysNumerator = _chronosService.GetDaysBetweenVna15AndDate(liquidationDate, vnaDto.ReferenceDate);

        // c) businessDaysDenominator = total de dias do mês do VNA (ou dias até o próximo dia 15?)
        // Neste exemplo, assumido que "DateTime.DaysInMonth(...)" serve de denominador simplificado
        int businessDaysDenominator = DateTime.DaysInMonth(vnaDto.ReferenceDate.Year, vnaDto.ReferenceDate.Month);
        if (businessDaysDenominator == 0) businessDaysDenominator = 1;

        // d) expo = (diasUteis1 / diasUteis2)
        decimal expo = (decimal)businessDaysNumerator / businessDaysDenominator;
        decimal factor = (decimal)Math.Pow((double)(1 + IpcaProjected), (double)expo);
        decimal vnaProjected = vnaBase * factor;

        // 4) Gera fluxos de pagamento (cupom semestral + principal) usando o VNA projetado
        var cashFlows = await GenerateCashFlowsAsync(bond, request, vnaProjected);

        // 5) Desconto (somar valor presente e calcular Duration)
        double rate = (double)request.Rate;
        double sumPV = 0.0;
        double weightedSum = 0.0;

        foreach (var cf in cashFlows)
        {
            double years = cf.BusinessDays / 252.0;
            double discountFactor = Math.Pow(1 + rate, years);
            double pv = (double)cf.FutureValue / discountFactor;

            cf.PresentValue = (decimal)pv;
            sumPV += pv;
            weightedSum += years * pv;
        }

        double duration = sumPV > 0 ? weightedSum / sumPV : 0.0;

        return new BondPricingResponse
        {
            CashFlowPayments = cashFlows,
            Duration = (decimal)duration,
            PresentValue = (decimal)sumPV
        };
    }

    /// <summary>
    /// Gera fluxos de cupom semestrais e principal no vencimento usando o VNA projetado.
    /// Se o vencimento cair em ano par => fev(2,15) e ago(8,15). Ano ímpar => mai(5,15), nov(11,15).
    /// </summary>
    private async Task<List<CashFlowPayment>> GenerateCashFlowsAsync(Bond bond, BondPricingRequest request, decimal vnaProjected)
    {
        var cashFlows = new List<CashFlowPayment>();

        bool isMaturityEvenYear = (bond.MaturityDate.Year % 2 == 0);

        // Meses de cupom (mês A e mês B semestral)
        (int MonthA, int DayA, int MonthB, int DayB) couponMonths =
            isMaturityEvenYear ? (2, 15, 8, 15) : (5, 15, 11, 15);

        DateTime couponDate = FindNextCouponDate(request.PurchaseDate, bond.MaturityDate, couponMonths);

        while (couponDate <= bond.MaturityDate)
        {
            // Cupom semestral => (vnaProjected * ((1 + AnnualCoupon)^0.5 - 1))
            decimal semiannualCouponValue = vnaProjected * ((decimal)Math.Pow(1 + AnnualCoupon, 0.5) - 1);

            int businessDays = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, couponDate);

            cashFlows.Add(new CashFlowPayment
            {
                Date = couponDate,
                BusinessDays = businessDays,
                FutureValue = semiannualCouponValue,
                PresentValue = 0m,
                Coupon = true
            });

            // Próxima data semestral
            couponDate = GetNextSemiannualDate(couponDate, couponMonths);
        }

        // Principal no vencimento -> vnaProjected
        int businessDaysToMaturity = await _chronosService.GetBusinessDaysAsync(request.PurchaseDate, bond.MaturityDate);
        cashFlows.Add(new CashFlowPayment
        {
            Date = bond.MaturityDate,
            BusinessDays = businessDaysToMaturity,
            FutureValue = vnaProjected,
            PresentValue = 0m,
            Coupon = false
        });

        return cashFlows;
    }

    private DateTime FindNextCouponDate(DateTime purchaseDate, DateTime maturityDate, (int MonthA, int DayA, int MonthB, int DayB) couponMonths)
    {
        DateTime candidateA = new DateTime(purchaseDate.Year, couponMonths.MonthA, couponMonths.DayA);
        DateTime candidateB = new DateTime(purchaseDate.Year, couponMonths.MonthB, couponMonths.DayB);

        DateTime firstCoupon;
        if (candidateA > purchaseDate.Date)
        {
            firstCoupon = candidateA;
        }
        else if (candidateB > purchaseDate.Date)
        {
            firstCoupon = candidateB;
        }
        else
        {
            firstCoupon = new DateTime(purchaseDate.Year + 1, couponMonths.MonthA, couponMonths.DayA);
        }

        if (firstCoupon > maturityDate)
            firstCoupon = maturityDate;

        return firstCoupon;
    }

    private DateTime GetNextSemiannualDate(DateTime currentCouponDate, (int MonthA, int DayA, int MonthB, int DayB) couponMonths)
    {
        int month = currentCouponDate.Month;
        int day = currentCouponDate.Day;
        int year = currentCouponDate.Year;

        if (month == couponMonths.MonthA && day == couponMonths.DayA)
        {
            return new DateTime(year, couponMonths.MonthB, couponMonths.DayB);
        }
        else
        {
            return new DateTime(year + 1, couponMonths.MonthA, couponMonths.DayA);
        }
    }
}
