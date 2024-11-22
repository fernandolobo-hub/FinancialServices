using FinancialServices.Application.Exceptions;
using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Application.Interfaces.Services;
using FinancialServices.Domain.Entities;
using FinancialServices.Domain.RequestObjects;
using Microsoft.AspNetCore.Mvc;

namespace FinancialServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicBondsDailyInfoController : ControllerBase
    {
        private readonly ILogger<PublicBondTypesController> _logger;
        private readonly IDailyBondsImportService _dailyBondsImportService;

        public PublicBondsDailyInfoController(ILogger<PublicBondTypesController> logger,
            IDailyBondsImportService dailyBondsImportService,
            IPublicBondsInfoService publicBondsInfoService)
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
            catch (CustomValidationException ex)
            {
                return BadRequest(ex.Message);
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
