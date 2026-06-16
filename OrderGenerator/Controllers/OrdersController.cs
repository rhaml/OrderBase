using Microsoft.AspNetCore.Mvc;
using OrderGenerator.DTOs;
using OrderGenerator.Services;

namespace OrderGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderGateway _orderGateway;

        public OrdersController(OrderGateway orderGateway)
        {
            _orderGateway = orderGateway;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);

            try
            {
                var response = await _orderGateway.SendOrderAsync(request, linkedCts.Token);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(504, new { Message = "Request timed out" });
            }
        }
    }
}
