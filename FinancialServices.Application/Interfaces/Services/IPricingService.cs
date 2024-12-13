using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Interfaces.Services
{
    public interface IPricingService
    {
        Task<IEnumerable<BondPricingResponse>> CalculatePrice(BondPricingRequest request);
    }
}
