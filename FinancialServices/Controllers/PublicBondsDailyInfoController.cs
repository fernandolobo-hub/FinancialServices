using PublicBonds.Application.Exceptions;
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
    public class PublicBondsDailyInfoController : ControllerBase
    {
        private readonly ILogger<PublicBondsInformationalController> _logger;
        private readonly IDailyBondsImportService _dailyBondsImportService;

        public PublicBondsDailyInfoController(ILogger<PublicBondsInformationalController> logger,
            IDailyBondsImportService dailyBondsImportService,
            IPublicBondsInformationalService publicBondsInfoService)
        {
            _logger = logger;
            _dailyBondsImportService = dailyBondsImportService;
        }

        //Description: returns all public bonds the system has data on.
        [HttpPost("ImportAllHistoricalDailyBondsInfo", Name = "Import")]
        public async Task<ActionResult> Post([FromBody] PublicBondHistoricalImportFilterRequest request)
        {

            try
            {
                await _dailyBondsImportService.ImportAllHistoricalDailyBondsData(request);
            }
            catch (DailyBondImportRequestValidationException ex)
            {
                return BadRequest(ResponseEnvelope<object>.Error(ex.ToString()));
            }
            catch(HttpRequestException)
            {
                return BadRequest();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: ", ex);
                throw;
            }
            
            return Created();
        }
    }
}
