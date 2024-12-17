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
        private readonly IVnaService _vnaService;

        public NtnbPricingStrategy(IChronosService chronosService, IVnaService vnaService)
        {
            _chronosService = chronosService;
            _vnaService = vnaService;
        }

        public Task<BondPricingResponse> CalculatePriceAsync(BondPricingRequest request)
        {
            var businessDays = _chronosService.GetBusinessDaysAsync(request.PurchaseDate, request.BondMaturityDate);

            throw new NotImplementedException();
        }
    }
}
