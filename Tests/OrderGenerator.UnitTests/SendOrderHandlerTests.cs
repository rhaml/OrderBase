using FluentAssertions;
using Moq;
using OrderGenerator.Application.Commands;
using OrderGenerator.Application.Handlers;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Models;

namespace OrderGenerator.UnitTests
{
    public class SendOrderHandlerTests
    {
        [Fact]
        public async Task Should_Return_ExecutionResult()
        {
            var gateway = new Mock<IFixGateway>();
            gateway.Setup(x => x.SendOrderAsync(It.IsAny<OrderRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ExecutionResult("1", "PETR4", 1000, true,null));
            var handler = new SendOrderHandler(gateway.Object);
            var result = await handler.Handle(new SendOrderCommand("PETR4", "Buy", 100, 10), default);
            result.Accepted.Should().BeTrue();
        }
    }
}