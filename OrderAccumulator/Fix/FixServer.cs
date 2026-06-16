using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;

namespace OrderAccumulator.Fix
{
    public class FixServer : IHostedService
    {
        private readonly FixApplication _application;
        private ThreadedSocketAcceptor? _theadedSocketAcceptor;

        public FixServer(FixApplication application)
        {
            _application = application;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var settings = new SessionSettings("quickfix.cfg");
            var storeFactory = new FileStoreFactory(settings);
            var logFactory = new FileLogFactory(settings);
            var messageFactory = new DefaultMessageFactory();

            _theadedSocketAcceptor = new ThreadedSocketAcceptor(_application, storeFactory, settings, logFactory, messageFactory);

            _theadedSocketAcceptor.Start();
            Console.WriteLine(" == FIX server started !== ");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _theadedSocketAcceptor?.Stop();

            return Task.CompletedTask;
        }
    }
}
