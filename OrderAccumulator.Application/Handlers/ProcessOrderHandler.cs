using MediatR;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Domain.Enums;
using OrderAccumulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Handlers
{
    public  class ProcessOrderHandler : IRequestHandler<ProcessOrderCommand, ExposureResult>
    {
        private readonly IExposureService _exposureService;
        public ProcessOrderHandler(IExposureService exposureService)
        {
            _exposureService = exposureService;
        }

        public async Task<ExposureResult> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = new Domain.Models.OrderRequest(request.Symbol, 
                request.Side.Equals("Buy", StringComparison.OrdinalIgnoreCase) ? OrderSide.Buy : OrderSide.Sell,
                request.Quantity, 
                request.Price);

            return await _exposureService.ProcessAsync(orderRequest, cancellationToken);
        }
}
}
