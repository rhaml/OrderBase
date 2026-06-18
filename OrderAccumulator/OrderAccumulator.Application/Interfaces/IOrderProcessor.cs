using OrderAccumulator.Application.Commands;
using OrderAccumulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Interfaces
{
    public interface IOrderProcessor
    {
        Task<ExposureResult> Process(ProcessOrderCommand processOrderCommand);
    }
}
