using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs;
using Shipping.BusinessLogicLayer.Services;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingsController : ControllerBase
    {
        private readonly GeneralSettingsService _generalSettingsService;
        public GeneralSettingsController(GeneralSettingsService generalSettingsService)
        {
            _generalSettingsService = generalSettingsService;
        }
        [HttpGet]
        public async Task<ActionResult<ReadGeneralSettingsDTO>> GetGeneralSettings()
        {
            try
            {
                var settings = await _generalSettingsService.GetGeneralSettingsAsync();
                return Ok(settings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGeneralSettings([FromBody] UpdateGeneralSettingsDTO settings)
        {
            if (settings == null)
            {
                return BadRequest("Invalid settings data.");
            }
            try
            {
                await _generalSettingsService.UpdateGeneralSettingsAsync(settings);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
