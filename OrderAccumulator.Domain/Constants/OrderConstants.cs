using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Domain.Constants
{
    public static class OrderConstants
    {
        public const decimal ExposureLimit = 100000000m;

        public static readonly string[] ValidSymbols = new[] { "PETR4", "VALE3", "VIIA4"};
       
    }
}
