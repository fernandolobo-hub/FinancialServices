using FluentValidation;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Exceptions;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Validators
{
    public class PricingRequestValidator : AbstractValidator<BondPricingRequest>
    {
        public PricingRequestValidator()
        {
            RuleFor(x => x.Rate).GreaterThan(0).WithMessage("Rate must be greater than 0");

            RuleFor(x => x.BondName)
                .NotEmpty()
                .WithMessage("Bond Name must be provided")
                .Must((x, bondName) => IsValidBond(bondName, x.BondMaturityDate))
                .WithMessage("Invalid Bond Name");

            RuleFor(x => x.ReferenceDate).NotEmpty().Must(IsValidDate).WithMessage("Date must be greater or equal than today");

            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }


        private bool IsValidDate(DateTime date)
        {
            //There should be a validation to check if the date passed is greater than the first date the bond was traded. 
            //However, we don't have tis information in the current context. Should be implemented at some point
            //Will just validate if the date is greater than today, akcnowledging it limitates the api usage not allowing to price bonds in the past
            return date >= DateTime.Today;
        }

        private bool IsValidBond(string bondName, DateTime maturityDate)
        {
            try
            {
                var bond = BondCaching.GetBondByNameAndMaturityDate(bondName, maturityDate);
                if (bond is null) return false;
                return true;
            }
            catch(InvalidBondTypeNameException)
            {
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Internal error validating bond name", ex);
                throw;
            }
        }
    }
}
