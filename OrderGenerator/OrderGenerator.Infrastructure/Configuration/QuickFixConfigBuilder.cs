using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Configuration
{
    public class QuickFixConfigBuilder
    {
        private readonly FixOptions _fixOptions;

        public QuickFixConfigBuilder(IOptions<FixOptions> fixOptions)
        {
            _fixOptions = fixOptions.Value;
        }

        public string Build()
        {
            var template = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Templates", "quickfix.template.cfg"));
            template = template.Replace("{{Host}}", _fixOptions.Host).Replace("{{Port}}", _fixOptions.Port.ToString());
            var generatedFile = Path.Combine(AppContext.BaseDirectory,"quickfix.generated.cfg");
            File.WriteAllText(generatedFile,template);

            return generatedFile;
        }
    }
}
