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

        [HttpGet("AvailableBondTypes", Name = "BondTypes")]
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

        [HttpGet("AvailableBonds", Name = "Bonds")]
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

        //Como é um endpoint basico, nao me preocuparei neste momento nos tratamentos especificos de possiveis exceções.
        [HttpGet("AvailableIndexes", Name="Indexers")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<IndexerResponseDto>>>> GetAvailableVnas()
        {
            try
            {
                var indexes = await _publicBondsService.GetAvailableIndexesAsync();
                if(indexes.Count() > 0)
                {
                    return ResponseEnvelope<IEnumerable<IndexerResponseDto>>.Ok(indexes);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Write("Error searching for available vnas", ex.Message);
                throw;
            }
        }
    }
}
