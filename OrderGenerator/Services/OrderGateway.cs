using OrderGenerator.DTOs;
using QuickFix;
using QuickFix.Fields;

namespace OrderGenerator.Services
{
    public class OrderGateway
    {
        private readonly Fix.FixApplication _fixApplication;
        private readonly ExecutionReportStore _executionReportStore;

        public OrderGateway(Fix.FixApplication fixApplication, ExecutionReportStore executionReportStore)
        {
            _fixApplication = fixApplication;
            _executionReportStore = executionReportStore;
        }
        public async Task<OrderResponse> SendOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            if (_fixApplication.SessionID == null)
            {
                throw new InvalidOperationException("FIX session is not established.");
            }
            var clOrdID = Guid.NewGuid().ToString();
            var side = request.Side == "BUY" ? Side.BUY : Side.SELL;
            var order = new QuickFix.FIX44.NewOrderSingle(
                new ClOrdID(clOrdID),
                new Symbol(request.Symbol),
                new Side(side),
                new TransactTime(DateTime.UtcNow),
                new OrdType(OrdType.LIMIT)
            );
            order.Set(new OrderQty(request.Quantity));
            order.Set(new Price(request.Price));
            try
            {
                Session.SendToTarget(order, _fixApplication.SessionID);
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("==session not found exception!==");
                Console.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return await _executionReportStore.WaitForExecutionReport(clOrdID, cancellationToken);
        }
    }
}
