using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Api.Contracts;
using OrderGenerator.Application.Commands;
using OrderGenerator.Domain.Models;

namespace OrderGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new SendOrderCommand(request.Symbol, request.Side, request.Quantity, request.Price), cancellationToken);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(504, new { Message = "Request timed out" });
            }
            catch (Exception ex)
            {
                return Ok(new ExecutionResult("", request.Symbol, 0, false, Message: ex.Message));
            }
        }
    }
}
