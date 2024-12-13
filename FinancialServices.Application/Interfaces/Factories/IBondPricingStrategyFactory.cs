using PublicBonds.Application.Interfaces.Strategies;
using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Interfaces.Factories
{
    public interface IBondPricingStrategyFactory
    {
        IBondPricingStrategy GetBondPricingStrategy(BondType bondType);
    }
}
