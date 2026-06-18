using MediatR;
using OrderGenerator.Domain.Enums;
using OrderGenerator.Application.Commands;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Application.Handlers
{
    public  class SendOrderHandler : IRequestHandler<SendOrderCommand, ExecutionResult>
    {
        private readonly IFixGateway _fixGateway;

        public SendOrderHandler(IFixGateway fixGateway)
        {
            _fixGateway = fixGateway;
        }

        public Task<ExecutionResult> Handle(SendOrderCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = new OrderRequest(request.Symbol,
                request.Side.Equals("Buy", StringComparison.OrdinalIgnoreCase) ? OrderSide.Buy : OrderSide.Sell,
                request.Quantity,
                request.Price);

            return _fixGateway.SendOrderAsync(orderRequest, cancellationToken);
        }
    }
}
