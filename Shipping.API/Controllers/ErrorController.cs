using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("test-error")]
        public IActionResult GetError()
        {
            throw new Exception("This is a test error");
        }
    }
}
