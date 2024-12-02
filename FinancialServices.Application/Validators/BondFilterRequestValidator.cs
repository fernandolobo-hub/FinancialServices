using FluentValidation;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.RequestObjects;

namespace PublicBonds.Application.Validators
{
    public class BondFilterRequestValidator : AbstractValidator<BondFilterRequest>
    {
        public BondFilterRequestValidator()
        {
            RuleFor(x => x.BondTypeName)
                .Must(IsValidBondType)
                .WithMessage("Invalid Bond Type"); // Altere para obter do .resx
        }

        private static bool IsValidBondType(string bondTypeName)
        {
            try
            {
                var bondType = BondTypeCaching.GetBondTypeByName(bondTypeName);

                return !string.IsNullOrEmpty(bondType.Name);
            }
            catch(Exception)
            {
                return false;
            }
            
        }
    }
}