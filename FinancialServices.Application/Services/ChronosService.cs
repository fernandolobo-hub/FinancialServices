using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;

namespace PublicBonds.Application.Services
{
    public class ChronosService : IChronosService
    {
        private readonly IHolidaysService _holidaysService;

        public ChronosService(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        public async Task<int> GetBusinessDaysAsync(DateTime referenceDate, DateTime endDate)
        {
            var holidays = await _holidaysService.GetHollidaysAsync(referenceDate, endDate);

            var liquidationDate = await GetNextBusinessDayAsync(referenceDate.AddDays(1));

            int businessDays = 0;
            DateTime currentDate = liquidationDate;

            while (currentDate < endDate)
            {
                if (IsBusinessDay(currentDate, holidays))
                {
                    businessDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return businessDays;
        }

        /// <summary>
        /// Retorna o próximo dia útil para a data fornecida. Se a data já for dia útil, retorna a própria data.
        /// </summary>
        public async Task<DateTime> GetNextBusinessDayAsync(DateTime date)
        {
            // Aqui precisamos estimar um período de busca de feriados (por ex. data até date.AddDays(365) ou algo coerente ao seu contexto).
            // Para simplificar, buscaremos feriados para um intervalo de alguns dias à frente:
            // Se a data pode avançar muito, considere buscar um range maior.
            var rangeEnd = date.AddDays(10); // por exemplo, 10 dias. Ajuste conforme necessidade.
            var holidays = await _holidaysService.GetHollidaysAsync(date, rangeEnd);

            while (!IsBusinessDay(date, holidays))
            {
                date = date.AddDays(1);

                // Se ultrapassar o range do feriado buscado, recarregue feriados em um range maior,
                // ou configure seu IHolidaysService para suportar a data indefinidamente.
                if (date > rangeEnd)
                {
                    rangeEnd = date.AddDays(10);
                    holidays = await _holidaysService.GetHollidaysAsync(date, rangeEnd);
                }
            }

            return date;
        }

        public int GetDaysBetweenVna15AndDate(DateTime referenceDate, DateTime vnaDate)
        {
            var vnaMidMonth = new DateTime(vnaDate.Year, vnaDate.Month, 15);

            int days = (referenceDate.Date - vnaMidMonth.Date).Days;
            return days;
        }

        public static bool IsBusinessDay(DateTime date, HashSet<DateTime> holidays)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            if (holidays.Contains(date.Date))
                return false;

            return true;
        }

        /// <summary>
        /// Retorna o dia 15 do mês "atual" (se referenceDate dia 20, ainda 15 do mês atual).
        /// Se quiser tratar dia 15 "passado" ou "futuro" dependendo do day>15, ajuste a lógica.
        /// </summary>
        public DateTime GetMidMonthDate(DateTime referenceDate)
        {
            // Ex.: sempre dia 15 do mesmo mês/ano da referenceDate
            return new DateTime(referenceDate.Year, referenceDate.Month, 15);
        }

        /// <summary>
        /// Retorna o dia 15 do mês seguinte de midMonthCurrent.
        /// </summary>
        public DateTime GetNextMidMonthDate(DateTime midMonthCurrent)
        {
            int nextMonth = midMonthCurrent.Month == 12 ? 1 : midMonthCurrent.Month + 1;
            int nextYear = midMonthCurrent.Month == 12 ? midMonthCurrent.Year + 1 : midMonthCurrent.Year;
            return new DateTime(nextYear, nextMonth, 15);
        }
    }
}
