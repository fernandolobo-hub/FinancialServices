using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Strategies
{
    public class NtnbPricingStrategy : IBondPricingStrategy
    {
        private readonly IChronosService _chronosService;

        public NtnbPricingStrategy(IChronosService chronosService)
        {
            _chronosService = chronosService;
        }

        public Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var businessDays = _chronosService.GetBusinessDaysAsync(request.ReferenceDate, request.BondMaturityDate);

            throw new NotImplementedException();
        }
    }
}
