using FluentValidation;
using PublicBonds.Domain.Enums;
using PublicBonds.Domain.RequestObjects;


namespace PublicBonds.Application.Validators
{
    public class VnaFilterRequestValidator : AbstractValidator<VnaFilterRequest>
    {
        public VnaFilterRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("EndDate is required");

            RuleFor(x => x.Indexer)
            .Must(BeAValidIndexerEnum)
            .WithMessage("The Indexer field must be a valid value (SELIC or IPCA).");
        }

        private bool BeAValidIndexerEnum(string indexerValue)
        {
            return Enum.TryParse(typeof(IndexerEnum), indexerValue, true, out _);
        }

    }
}
