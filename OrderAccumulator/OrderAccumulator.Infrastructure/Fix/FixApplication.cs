using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Models;
using OrderAccumulator.Services;
using QuickFix;
using QuickFix.Fields;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrderAccumulator.Infrastructure.Fix
{
    public class FixApplication : MessageCracker, IApplication
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FixApplication> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FixApplication(IMediator mediator, ILogger<FixApplication> logger, IServiceScopeFactory scopeFactory)
        {
            _mediator = mediator;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void OnMessage(QuickFix.FIX44.NewOrderSingle order, SessionID sessionID)
        {
            Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<IOrderProcessor>();

                    var command = new ProcessOrderCommand
                    (
                        order.Symbol.Value,
                        order.Side.Value == Side.BUY ? "Buy" : "Sell",
                        order.OrderQty.Value,
                        order.Price.Value
                    );
                    var result = await processor.Process(command);
                   
                    var report = BuildExecutionReport(order, result);

                    Session.SendToTarget(report, sessionID);
                }
                catch (Exception ex)
                {
                    var exposureResult = new ExposureResult(false, order.Symbol.Value, 0, 0, 0, ex.Message);
                    var report = BuildExecutionReport(order, exposureResult);
                    Session.SendToTarget(report, sessionID);
                    _logger.LogError(ex.ToString());
                }
            });
        }

        private QuickFix.FIX44.ExecutionReport BuildExecutionReport(QuickFix.FIX44.NewOrderSingle order, ExposureResult exposureResult)
        {
            var report = new QuickFix.FIX44.ExecutionReport(
                new OrderID(Guid.NewGuid().ToString()),
                new ExecID(Guid.NewGuid().ToString()),
                new ExecType(exposureResult.Accepted ? ExecType.NEW : ExecType.REJECTED),
                new OrdStatus(exposureResult.Accepted ? OrdStatus.NEW : OrdStatus.REJECTED),
                order.Symbol,
                order.Side,
                new LeavesQty(exposureResult.Accepted ? order.OrderQty.Value : 0),
                new CumQty(exposureResult.NewExposure),
                new AvgPx(0)
            );

            report.Set(order.ClOrdID);

            if (!exposureResult.Accepted)
            {
                report.Set(new Text(exposureResult.RejectionReason));
            }

            return report;
        }

        #region Application Methods

        public void FromApp(Message msg, SessionID sessionID)
        {
            Crack(msg, sessionID);
        }
        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) {
            _logger.LogInformation("Logon: {SessionID}", sessionID);
        }
        public void FromAdmin(Message msg, SessionID sessionID) { }
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { }

        #endregion
    }
}
