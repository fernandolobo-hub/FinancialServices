using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Application.Interfaces.Services;
using FinancialServices.Domain.Entities;
using FinancialServices.Domain.RequestObjects;
using FinancialServices.Domain.ResponseObjects.Temps;
using Microsoft.AspNetCore.Mvc;

namespace FinancialServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicBondTypesController : ControllerBase
    {
        private readonly ILogger<PublicBondTypesController> _logger;
        private readonly IPublicBondsInfoService _publicBondsService;


        public PublicBondTypesController(ILogger<PublicBondTypesController> logger, IPublicBondsInfoService publicBondsInfoService, IDailyBondsImportService dailyBondsImportService)
        {
            _logger = logger;
            _publicBondsService = publicBondsInfoService;
        }

        //Description: returns all public bonds the system has data on.
        [HttpGet("AvailableBonds", Name = "Bonds")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondType>>>> Get([FromQuery] PublicBondsFilterRequest request)
        {

            var bonds = await _publicBondsService.GetAllAvailableBondTypes();
            if (bonds == null || !bonds.Any())
            {
                return NotFound();
            }
            var result = ResponseEnvelope<IEnumerable<BondType>>.Ok(bonds);
            return Ok(result);
        }
    }
}
