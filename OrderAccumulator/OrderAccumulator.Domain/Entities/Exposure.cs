using OrderAccumulator.Domain.Constants;
using OrderAccumulator.Domain.Enums;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Domain.Entities
{
    public class Exposure
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }

        private Exposure() 
        { 
            Symbol = string.Empty;
        }

        public Exposure(string symbol, decimal value = 0)
        {
            if(string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be null or empty.", nameof(symbol));
            
            Id = Guid.NewGuid();
            Symbol = symbol;
            Value = value;
        }

        public ExposureResult ApplyOrder(OrderRequest orderRequest)
        {
            decimal delta = orderRequest.Quantity * orderRequest.Price * (orderRequest.Side == OrderSide.Buy ? 1 : -1);
            decimal currentExposure = Value;
            decimal newExposure = currentExposure + delta;
            if (Math.Abs(newExposure) > OrderConstants.ExposureLimit)
            {
                return new ExposureResult(
                    Accepted: false,
                    Symbol: Symbol,
                    CurrentExposure: currentExposure,
                    NewExposure: currentExposure,
                    Delta: delta,
                    RejectionReason: $"Exposure limit exceeded for symbol {Symbol}. Current exposure: {currentExposure}, New exposure: {newExposure}"
                );
            }
            
            Value = newExposure;

            return new ExposureResult(
                Accepted: true,
                Symbol: Symbol,
                CurrentExposure: currentExposure,
                NewExposure: newExposure,
                Delta: delta
            );
        }
    }
}
