using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Infrastructure.Fix;

namespace OrderGenerator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FixController : ControllerBase
    {
        private readonly FixApplication _fixApplication;

        public FixController(FixApplication fixApplication)
        {
            _fixApplication = fixApplication;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { Status = _fixApplication.isConnected ? "Connected" : "Disconnected" });
        }
    } 
      
}
