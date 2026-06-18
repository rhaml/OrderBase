
using Microsoft.Extensions.Logging;
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
    public  class FixApplication : MessageCracker, IApplication
    {
        private readonly ExecutionReportStore _executionReportStore;
        private readonly ILogger<FixApplication> _logger;

        public SessionID? SessionID { get; private set; }
        public bool isConnected => SessionID != null;
        public FixApplication(ExecutionReportStore store, ILogger<FixApplication> logger)
        {
            _executionReportStore = store;
            _logger = logger;
        }

        public void OnMessage(QuickFix.FIX44.ExecutionReport report, SessionID sessionID)
        {
            var clOrdID = report.ClOrdID.Value;
            var accepted = report.ExecType.Value == ExecType.NEW;
            string? message = null;

            if(report.IsSetText())
            {
                message = report.Text.Value;
            }

            var executionResult = new ExecutionResult(clOrdID, accepted, message);

            _executionReportStore.Complete(executionResult);
            _logger.LogInformation("Received execution report for order {ClOrdID}: Accepted={Accepted}, Message={Message}", clOrdID, accepted, message);
        }

        #region Application Methods

        public void FromApp(Message msg, SessionID sessionID)
        {
            Crack(msg, sessionID);
        }
        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) 
        {
            _logger.LogInformation("FIX Session {SessionID} logged out.", sessionID);
            SessionID = null;
        }
        public void OnLogon(SessionID sessionID)
        {
            SessionID = sessionID;
            _logger.LogInformation("FIX Logon successful. SessionID: {SessionID}", sessionID);
        }
        public void FromAdmin(Message msg, SessionID sessionID) { }
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { }

        #endregion
    }
}
