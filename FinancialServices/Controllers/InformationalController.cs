using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.RequestObjects;
using Microsoft.AspNetCore.Mvc;
using PublicBonds.Domain.ResponseObjects;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.Exceptions.Request;

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
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondType>>>> GetBondTypes()
        {
            try
            {
                var bondTypes = await _publicBondsService.GetAvailableBondTypes();
                if (bondTypes == null || !bondTypes.Any())
                {
                    return NotFound();
                }
                var result = ResponseEnvelope<IEnumerable<BondTypeResponseDto>>.Ok(bondTypes);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao obter os bond types: ", ex);
                return StatusCode(500, "Internal error. We apologize for the inconvenience.");
            }
        }

        [HttpGet("Bonds", Name = "Bonds")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<BondResponseDto>>>> GetBonds([FromQuery] BondFilterRequest request)
        {
            try
            {
                var bonds = await _publicBondsService.GetBondsAsync(request);
                if (bonds == null || !bonds.Any())
                {
                    return NotFound();
                }
                var response = ResponseEnvelope<IEnumerable<BondResponseDto>>.Ok(bonds);
                return Ok(response);
            }
            catch(BondRequestValidationException ex)
            {
                var errorResponse = ResponseEnvelope<object>.Error(ex.Message);
                return BadRequest(errorResponse);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao obter os bonds: ", ex);
                return StatusCode(500);
            }
            
        }
    }
}
