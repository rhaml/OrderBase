using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;
using QuickFix.Transport;

namespace OrderGenerator.Fix
{
    public class FixClient : IHostedService
    {
        private readonly FixApplication _fixApplication;
        private SocketInitiator? _initiator;

        public FixClient(FixApplication fixApplication)
        {
            _fixApplication = fixApplication;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var settings = new SessionSettings("quickfix.cfg");

            var storeFactory = new FileStoreFactory(settings);
            var logFactory = new FileLogFactory(settings);
            var messageFactory = new DefaultMessageFactory();

            _initiator = new SocketInitiator(_fixApplication, storeFactory, settings, logFactory, messageFactory);
            _initiator.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _initiator?.Stop();
            return Task.CompletedTask;
        }
    }
}
