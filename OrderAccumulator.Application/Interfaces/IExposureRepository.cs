using OrderAccumulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Interfaces
{
    public interface IExposureRepository
    {
        Task<Exposure?> GetExposureBySymbolAsync(string symbol, CancellationToken cancellationToken);
        Task Add(Exposure exposure);
        Task Update(Exposure exposure);
    }
}
