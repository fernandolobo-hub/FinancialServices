using PublicBonds.Application.Interfaces.Factories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Application.Strategies;
using PublicBonds.Domain.Entities;


namespace PublicBonds.Application.Factories
{
    public class BondPricingStrategyFactory : IBondPricingStrategyFactory
    {
        private readonly IChronosService _chronosService;

        public BondPricingStrategyFactory(IChronosService chronosService)
        {
            _chronosService = chronosService;
        }

        public IBondPricingStrategy GetBondPricingStrategy(BondType bondType)
        {
            switch (bondType.Category)
            {
                case "LTN":
                    return new LtnPricingStrategy(_chronosService);
                case "NTN-F":
                    return new NtnfPricingStrategy(_chronosService);
                default:
                    throw new ArgumentException("Invalid bond type");
            }
        }
    }
}
