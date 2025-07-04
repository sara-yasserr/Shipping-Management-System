using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateLimiterController : ControllerBase
    {
        [EnableRateLimiting("StrictPolicy")]
        [HttpGet("limited-endpoint")]
        public IActionResult GetData()
        {
            return Ok("This endpoint is rate-limited.");
        }

    }
}
