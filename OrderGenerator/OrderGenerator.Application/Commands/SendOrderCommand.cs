using MediatR;
using OrderGenerator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Application.Commands
{
    public  record SendOrderCommand(string Symbol,
        string Side,
        decimal Quantity,
        decimal Price
    ) : IRequest<ExecutionResult>;
}
