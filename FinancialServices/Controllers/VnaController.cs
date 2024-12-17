using Microsoft.AspNetCore.Mvc;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Exceptions.Request;
using PublicBonds.Domain.RequestObjects;
using PublicBonds.Domain.ResponseObjects;

namespace PublicBonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VnaController : ControllerBase
    {
        private readonly ILogger<VnaController> _logger;
        private readonly IVnaService _vnaService;

        public VnaController(ILogger<VnaController> logger, IVnaService vnaService)
        {
            _logger = logger;
            _vnaService = vnaService;
        }

        [HttpGet("Vna", Name = "Vna")]
        public async Task<ActionResult<ResponseEnvelope<IEnumerable<VnaResponseDto>>>> GetVna([FromQuery] VnaFilterRequest request)
        {
            try
            {
                var vna = await _vnaService.GetVnasAsync(request);
                if (vna == null || !vna.Any())
                {
                    return NoContent();
                }
                var response = ResponseEnvelope<IEnumerable<VnaResponseDto>>.Ok(vna);
                return Ok(response);
            }
            catch (VnaRequestValidationException ex)
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
