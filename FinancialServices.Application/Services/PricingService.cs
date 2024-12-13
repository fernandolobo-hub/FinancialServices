using FluentValidation;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Factories;
using PublicBonds.Application.Interfaces.Factories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Persistance;
using PublicBonds.Domain.Exceptions.Request;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Services
{
    public class PricingService :IPricingService
    {
        private readonly IValidator<BondPricingRequest> _bondPricingRequestValidator;
        private readonly IChronosService _chronosService;
        private readonly IBondPricingStrategyFactory _bondPricingStrategyFactory;

        public PricingService(IValidator<BondPricingRequest> bondPricingRequestValidator,
            IChronosService chronosService,
            IBondPricingStrategyFactory bondPricingStrategyFactory)
        {
            _bondPricingRequestValidator = bondPricingRequestValidator;
            _chronosService = chronosService;
            _bondPricingStrategyFactory = bondPricingStrategyFactory;
        }

        public async Task<IEnumerable<BondPricingResponse>> CalculatePrice(BondPricingRequest request)
        {
            var validationResult = _bondPricingRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                throw new PublicBondsHistoricalDataRequestValidationException("Request Validation Error", errors);
            }

            var bond = BondCaching.GetBondByNameAndMaturityDate(request.BondName, request.BondMaturityDate);


            var businessDays = await _chronosService.GetBusinessDaysAsync(request.ReferenceDate, bond.MaturityDate);

            var pricingStrategy = _bondPricingStrategyFactory.GetBondPricingStrategy(bond.Type);

            var response = await pricingStrategy.CalculatePriceAsync(request);

            return new List<BondPricingResponse> { response };
        }
    }
}
