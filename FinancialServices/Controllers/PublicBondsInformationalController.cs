using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using PublicBonds.Domain.ResponseObjects.Temps;
using Microsoft.AspNetCore.Mvc;

namespace PublicBonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicBondsInformationalController : ControllerBase
    {
        private readonly ILogger<PublicBondsInformationalController> _logger;
        private readonly IPublicBondsInformationalService _publicBondsService;


        public PublicBondsInformationalController(ILogger<PublicBondsInformationalController> logger, IPublicBondsInformationalService publicBondsInfoService, IDailyBondsImportService dailyBondsImportService)
        {
            _logger = logger;
            _publicBondsService = publicBondsInfoService;
        }

        //Description: returns all public bonds the system has data on.
        [HttpGet("BondTypes", Name = "BondTypes")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondType>>>> GetBondTypes([FromQuery] PublicBondsFilterRequest request)
        {
            var bondTypes = await _publicBondsService.GetAllAvailableBondTypes();
            if (bondTypes == null || !bondTypes.Any())
            {
                return NotFound();
            }
            var result = ResponseEnvelope<IEnumerable<BondType>>.Ok(bondTypes);
            return Ok(result);
        }

        [HttpGet("Bonds", Name = "Bonds")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<Bond>>>> GetBonds([FromQuery] PublicBondsFilterRequest request)
        {

            var bonds = await _publicBondsService.GetAllAvailableBonds();
            if (bonds == null || !bonds.Any())
            {
                return NotFound();
            }
            var result = ResponseEnvelope<IEnumerable<Bond>>.Ok(bonds);
            return Ok(result);
        }
    }
}
