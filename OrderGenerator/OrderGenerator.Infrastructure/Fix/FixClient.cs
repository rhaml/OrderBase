using Microsoft.Extensions.Hosting;
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

        public FixClient(FixApplication application)
        {
            _application = application;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "quickfix.cfg");
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
