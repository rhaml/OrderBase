using Microsoft.Extensions.Hosting;
using OrderGenerator.Infrastructure.Configuration;
using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;
using QuickFix.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Fix
{
    public  class FixClient : IHostedService
    {
        private readonly FixApplication _application;
        private SocketInitiator? _socketInitiator;
        private readonly QuickFixConfigBuilder _configBuilder;

        public FixClient(FixApplication application, QuickFixConfigBuilder configBuilder)
        {
            _application = application;
            _configBuilder = configBuilder;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cfgPath = _configBuilder.Build();
            var path = Path.Combine(AppContext.BaseDirectory, cfgPath);
            var settings = new SessionSettings(path);
            var storeFactory = new FileStoreFactory(settings);
            var logFactory = new FileLogFactory(settings);
            var messageFactory = new DefaultMessageFactory();
            _socketInitiator = new SocketInitiator(_application, storeFactory, settings, logFactory, messageFactory);
            _socketInitiator.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _socketInitiator?.Stop();
            return Task.CompletedTask;
        }
    }
}
