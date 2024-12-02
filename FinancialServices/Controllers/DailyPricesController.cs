using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using Microsoft.AspNetCore.Mvc;
using PublicBonds.Domain.ResponseObjects;
using PublicBonds.Domain.Exceptions.Request;

namespace PublicBonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyPricesController : ControllerBase
    {
        private readonly ILogger<InformationalController> _logger;
        private readonly IDailyPricesService _dailyBondsImportService;

        public DailyPricesController(ILogger<InformationalController> logger,
            IDailyPricesService dailyBondsImportService,
            IInformationalService publicBondsInfoService)
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
