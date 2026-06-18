using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Domain.Exceptions
{
    public  class ExposureLimitExceedException : Exception
    {
        public ExposureLimitExceedException(string symbol, decimal currentExposure, decimal newExposure)
            : base($"Exposure limit exceeded for symbol {symbol}. Current exposure: {currentExposure}, New exposure: {newExposure}")
        {
        }
    }
}
