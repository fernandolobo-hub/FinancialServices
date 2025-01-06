using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.RequestObjects;
using PublicBonds.Domain.ResponseObjects;

namespace PublicBonds.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {

        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondPricingResponse>>>> Get([FromQuery] BondPricingRequest request)
        {

            try
            {
                var pricingResponses = await _pricingService.CalculatePrice(request);

                return Ok(ResponseEnvelope<IEnumerable<BondPricingResponse>>.Ok(pricingResponses));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
