using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using Microsoft.AspNetCore.Mvc;
using PublicBonds.Domain.ResponseObjects;
using PublicBonds.Domain.Exceptions.Request;
using PublicBonds.Application.DTOs.Response;

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
        [HttpPost("Import", Name = "Import")]
        public async Task<ActionResult> Post([FromBody] PublicBondHistoricalImportFilterRequest request)
        {

            try
            {
                await _dailyBondsImportService.ImportAllHistoricalDailyBondsData(request);
            }
            catch (DailyBondImportRequestValidationException ex)
            {
                return BadRequest(ResponseEnvelope<object>.Error(ex.Message));
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
            
            return Ok(ResponseEnvelope<object>.Ok(message: $"{request.BondName} successfully imported"));
        }

        [HttpGet("HistoricalPrices", Name = "HistoricalPrices")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<DailyBondInfoDto>>>> GetHistoricalPrices([FromQuery] PublicBondsHistoricalDataFilterRequest request)
        {
            try
            {
                var historicalPrices = await _dailyBondsImportService.GetHistoricalPrices(request);
                if (historicalPrices == null || !historicalPrices.Any())
                {
                    return NotFound();
                }
                var response = ResponseEnvelope<IEnumerable<DailyBondInfoDto>>.Ok(historicalPrices);
                return Ok(response);
            }
            catch (PublicBondsHistoricalDataRequestValidationException ex)
            {
                return BadRequest(ResponseEnvelope<object>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: ", ex);
                throw;
            }
        }


    }
}
