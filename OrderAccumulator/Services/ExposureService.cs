using OrderAccumulator.Models;
using System.Collections.Concurrent;

namespace OrderAccumulator.Services
{
    public class ExposureService
    {
        private const decimal ExposureLimit = 1000000000m;
        private readonly ConcurrentDictionary<string, decimal> _financialExposure = new();

        public ExposureResult ProcessOrder(string symbol, char side, decimal quantity, decimal price)
        {
            decimal delta = quantity * price * (price == QuickFix.Fields.Side.BUY ? 1 : -1);

            while (true)
            {
                decimal currentExposure = _financialExposure.GetOrAdd(symbol, 0);

                decimal candidateExposure = currentExposure + delta;

                if (candidateExposure > ExposureLimit)
                {
                    return new ExposureResult(false, symbol,currentExposure, currentExposure, delta, "Order rejected due to exposure limit breach.");
                }

                if (_financialExposure.TryUpdate(symbol, candidateExposure, currentExposure))
                {
                    return new ExposureResult(true, symbol, currentExposure, candidateExposure, delta);
                }
            }
        }

        public decimal GetExposure(string symbol)
        {
            return _financialExposure.GetValueOrDefault(symbol);
        }
    }
}
