using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAccumulator.Infrastructure.Data;

namespace OrderAccumulator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExposureController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public ExposureController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Exposures.OrderBy(x => x.Symbol).Select(x => new {x.Symbol, x.Value }).ToListAsync();
            return Ok(result);
        }
    }
}
