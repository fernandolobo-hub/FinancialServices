using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Strategies
{
    public class NtnbPrincipalPricingStrategy : IBondPricingStrategy
    {
        private readonly IChronosService _chronosService;

        public NtnbPrincipalPricingStrategy(IChronosService chronosService)
        {
            _chronosService = chronosService;
        }

        public Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
