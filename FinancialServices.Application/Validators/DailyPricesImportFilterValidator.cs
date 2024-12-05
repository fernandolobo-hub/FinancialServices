using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using FluentValidation;

namespace PublicBonds.Application.Validators
{
    public class DailyPricesImportFilterValidator : AbstractValidator<PublicBondHistoricalImportFilterRequest>
    {
        public DailyPricesImportFilterValidator()
        {
            RuleFor(x => x.Year)
                .Must((x, year) => IsValidYear(year, x.BondName)) // Passando o objeto completo para validação
                .WithMessage("Invalid Year"); // Altere para obter do .resx

            RuleFor(x => x.BondName)
                .Must(IsValidBond)
                .WithMessage("Bond Type informed not found"); // Altere para obter do .resx
        }

        private static bool IsValidYear(int? year, string? bondName)
        {
            if (year == null) return true;
            if (String.IsNullOrWhiteSpace(bondName))
            {
                return year >= 2000 && year <= DateTime.Now.Year;
            }
            
            var bond = BondTypeCaching.GetBondTypeByName(bondName);
            return year >= bond.FirstTradedAt && year <= DateTime.Now.Year;
        }

        private static bool IsValidBond(string? bondName)
        {
            if (string.IsNullOrEmpty(bondName))
            {
                return true;
            }

            var availableBonds = BondTypeCaching.BondTypes;
            return availableBonds is not null && availableBonds.Any(b => b.Name.Equals(bondName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
