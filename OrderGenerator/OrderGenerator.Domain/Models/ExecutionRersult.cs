using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Domain.Models
{
    public  record ExecutionResult(string ClOrdId,
        bool Accepted,
        string? Message);
}
