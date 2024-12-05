using FluentValidation;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Exceptions;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Validators
{
    public class DailyPricesInfoRequestValidator : AbstractValidator<PublicBondsHistoricalDataFilterRequest>
    {
        public DailyPricesInfoRequestValidator()
        {
            RuleFor(x => x.BondName)
                .Must((x, bondName) => IsValidBond(bondName, x.MaturityDate))
                .WithMessage("Invalid Bond Name"); // Altere para obter do .resx
        }

        private static bool IsValidBond(string bondName, DateTime maturityDate)
        {
            try
            {
                var bond = BondCaching.GetBondByNameAndMaturityDate(bondName, maturityDate);

                return !string.IsNullOrEmpty(bond.Name);
            }
            catch (BondValidationException)
            {
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error to validate bond", ex.Message);
                throw;
            }


        }
    }
}
