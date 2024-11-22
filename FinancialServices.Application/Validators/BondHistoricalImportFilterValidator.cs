using FinancialServices.Application.Interfaces.Services;
using FinancialServices.Application.Persistance;
using FinancialServices.Domain.Entities;
using FinancialServices.Domain.RequestObjects;
using FluentValidation;

namespace FinancialServices.Application.Validators
{
    public class BondHistoricalImportFilterValidator : AbstractValidator<PublicBondHistoricalImportFilterRequest>
    {
        public BondHistoricalImportFilterValidator()
        {
            RuleFor(x => x.Year)
                .Must((x, year) => IsValidYear(year, x.BondName)) // Passando o objeto completo para validação
                .WithMessage("Ano inválido"); // Altere para obter do .resx

            RuleFor(x => x.BondName)
                .Must(IsValidBond)
                .WithMessage("Título informado não encontrado"); // Altere para obter do .resx
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
