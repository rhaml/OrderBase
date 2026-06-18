using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Telemetry
{
    public static class TelemetrySource
    {
        public static readonly ActivitySource Source = new("OrderGenerator");
    }
}
