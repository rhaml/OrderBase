using OrderGenerator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Application.Interfaces
{
    public interface IFixGateway
    {
        Task<ExecutionResult> SendOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken);  
    }
}
