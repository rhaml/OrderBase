using OrderAccumulator.Models;
using OrderAccumulator.Services;
using QuickFix;
using QuickFix.Fields;

namespace OrderAccumulator.Fix
{
    public class FixApplication : MessageCracker, IApplication
    {
        private readonly ExposureService _exposureService;

        public SessionID? SessionID { get; private set; }

        public FixApplication(ExposureService exposureService)
        {
            _exposureService = exposureService;
        }

        public void OnMessage(QuickFix.FIX44.NewOrderSingle order, SessionID sessionID)
        {
            string symbol = order.Symbol.Value;
            char side = order.Side.Value;
            decimal quantity = order.OrderQty.Value;
            decimal price = order.Price.Value;

            var exposureResult = _exposureService.ProcessOrder(symbol, side, quantity, price);
            var exposureReport = BuildExecutionReport(order, exposureResult);

            try
            {
                Session.SendToTarget(exposureReport, sessionID);
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("==session not found exception!==");
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
                new CumQty(0),
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
            SessionID = sessionID;
            Console.WriteLine($"Session {sessionID} logged in.");
        }
        public void FromAdmin(Message msg, SessionID sessionID) { }
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { }

        #endregion
    }
}
