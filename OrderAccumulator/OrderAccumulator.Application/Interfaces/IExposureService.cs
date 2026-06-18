using OrderAccumulator.Domain.Models;
using OrderAccumulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Interfaces
{
    public interface IExposureService
    {
        Task<ExposureResult> ProcessAsync(OrderRequest order, CancellationToken cancellationToken);
    }
}
