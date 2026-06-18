using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Configuration
{
    public class FixOptions
    {
        public const string SectionName = "Fix";
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
