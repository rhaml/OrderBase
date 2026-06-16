using OrderGenerator.DTOs;
using OrderGenerator.Services;
using QuickFix;
using QuickFix.Fields;

namespace OrderGenerator.Fix
{
    public class FixApplication : MessageCracker, IApplication
    {
        private readonly ExecutionReportStore _store;

        public SessionID? SessionID { get; private set; }
        public FixApplication(ExecutionReportStore store)
        {
            _store = store;
        }

        public void OnMessage(QuickFix.FIX44.ExecutionReport report, SessionID sessionID)
        {
            var clOrdID = report.ClOrdID.Value;
            var exectType = report.ExecType.Value;
            var message = report.IsSetText() ? report.Text.Value : null;

            var orderResponse = new OrderResponse
            {
                ClOrdId = clOrdID,
                Status = exectType == ExecType.NEW ? "Accepted" : "Rejected",
                Message = message
            };

            _store.CompleteExecutionReport(clOrdID, orderResponse);
        }

        #region Application Methods

        public void FromApp(Message msg, SessionID sessionID)
        {
            Crack(msg, sessionID);
        }
        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID)
        {
            SessionID = sessionID;
        }
        public void FromAdmin(Message msg, SessionID sessionID) { }
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { }

        #endregion
    }
}
