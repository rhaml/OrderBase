using MediatR;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Processors
{
    public class OrderProcessor : IOrderProcessor
    {
        private readonly IMediator _mediator;

        public OrderProcessor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<ExposureResult> Process(ProcessOrderCommand processOrderCommand)
        {
            return _mediator.Send(processOrderCommand);
        }
    }
}
