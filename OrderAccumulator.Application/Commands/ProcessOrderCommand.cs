using OrderAccumulator.Models;
using MediatR;

namespace OrderAccumulator.Application.Commands
{
    public  record ProcessOrderCommand(
        string Symbol,
        string Side,
        decimal Quantity,
        decimal Price
    ) : IRequest<ExposureResult>;
}
