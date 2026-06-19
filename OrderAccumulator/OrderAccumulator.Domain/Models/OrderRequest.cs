using OrderAccumulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Domain.Models
{
    public record OrderRequest(string Symbol,
        OrderSide Side,
        decimal Quantity,
        decimal Price
    );
}
