using PublicBonds.Application.Interfaces.Factories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Services;
using PublicBonds.Application.Strategies;
using PublicBonds.Domain.Entities;


namespace PublicBonds.Application.Factories
{
    public class BondPricingStrategyFactory : IBondPricingStrategyFactory
    {
        private readonly IChronosService _chronosService;
        private readonly IVnaService _vnaService;
        public BondPricingStrategyFactory(IChronosService chronosService,
                                          IVnaService vnaService)
        {
            _chronosService = chronosService;
            _vnaService = vnaService;
        }

        public IBondPricingStrategy GetBondPricingStrategy(BondType bondType)
        {
            return bondType.Category switch
            {
                "LTN" => new LtnPricingStrategy(_chronosService),
                "NTN-F" => new NtnfPricingStrategy(_chronosService),
                "NTN-B Principal" => new NtnbPrincipalStrategy(_chronosService, _vnaService),
                "NTN-B" => new NtnbPricingStrategy(_chronosService, _vnaService),
                _ => throw new ArgumentException("Invalid bond type")
            };
        }
    }
}
