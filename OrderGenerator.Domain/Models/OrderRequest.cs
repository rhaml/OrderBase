using OrderGenerator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Domain.Models
{
    public  record OrderRequest(string Symbol,
         OrderSide Side,
         decimal Quantity,
         decimal Price
     );
}
