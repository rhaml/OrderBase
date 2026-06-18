using Microsoft.Extensions.Logging;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Domain.Enums;
using OrderGenerator.Domain.Models;
using QuickFix;
using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Fix
{
    public  class FixGateway : IFixGateway
    {
        private readonly FixApplication _fixApplication;
        private readonly ExecutionReportStore _executionReportStore;
        private readonly ILogger<FixGateway> _logger;

        public FixGateway(FixApplication fixApplication, ExecutionReportStore executionReportStore, ILogger<FixGateway> logger)
        {
            _fixApplication = fixApplication;
            _executionReportStore = executionReportStore;
            _logger = logger;
        }

        public async Task<ExecutionResult> SendOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

                if (!_fixApplication.isConnected)
                    throw new InvalidOperationException("FIX Session is not connected.");

                using var activity = Telemetry.TelemetrySource.Source.StartActivity("SendOrder");
                string clOrdId = Guid.NewGuid().ToString();
                var fixOrder = BuildOrder(clOrdId, orderRequest);
                var waitTask = _executionReportStore.WaitAsync(clOrdId, linkedCts.Token);
                Session.SendToTarget(fixOrder, _fixApplication.SessionID!);
                return await waitTask;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending order via FIX gateway.");
                throw;
            }
        }

        private QuickFix.FIX44.NewOrderSingle BuildOrder(string clOrdId, OrderRequest orderRequest)
        {
            char side = orderRequest.Side == OrderSide.Buy ? Side.BUY : Side.SELL;

            var message = new QuickFix.FIX44.NewOrderSingle(
                new ClOrdID(clOrdId),
                new Symbol(orderRequest.Symbol),
                new Side(side),
                new TransactTime(DateTime.UtcNow),
                new OrdType(TriggerOrderType.LIMIT)
            );

            message.Set(new OrderQty(orderRequest.Quantity));
            message.Set(new Price(orderRequest.Price));
            return message;
        }
    }
}
