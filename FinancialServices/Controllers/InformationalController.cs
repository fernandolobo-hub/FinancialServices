using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using Microsoft.AspNetCore.Mvc;
using PublicBonds.Domain.ResponseObjects;
using PublicBonds.Application.DTOs.Response;

namespace PublicBonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InformationalController : ControllerBase
    {
        private readonly ILogger<InformationalController> _logger;
        private readonly IInformationalService _publicBondsService;


        public InformationalController(ILogger<InformationalController> logger, IInformationalService publicBondsInfoService, IDailyPricesService dailyBondsImportService)
        {
            _logger = logger;
            _publicBondsService = publicBondsInfoService;
        }

        //Description: returns all public bonds the system has data on.
        [HttpGet("BondTypes", Name = "BondTypes")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondTypeResponseDto>>>> GetBondTypes()
        {
            var bondTypes = await _publicBondsService.GetAvailableBondTypes();
            if (bondTypes == null || !bondTypes.Any())
            {
                return NotFound();
            }
            var result = ResponseEnvelope<IEnumerable<BondTypeResponseDto>>.Ok(bondTypes);
            return Ok(result);
        }

        [HttpGet("Bonds", Name = "Bonds")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<Bond>>>> GetBonds([FromQuery] BondFilterRequest request)
        {

            var bonds = await _publicBondsService.GetAvailableBonds(request);
            if (bonds == null || !bonds.Any())
            {
                return NotFound();
            }
            var result = ResponseEnvelope<IEnumerable<Bond>>.Ok(bonds);
            return Ok(result);
        }
    }
}
