using OrderGenerator.DTOs;
using System.Collections.Concurrent;

namespace OrderGenerator.Services
{
    public class ExecutionReportStore
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<OrderResponse>> _pending = new();

        public Task<OrderResponse> WaitForExecutionReport(string clOrdID, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<OrderResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pending.TryAdd(clOrdID, tcs);
            cancellationToken.Register(() =>
            {
                if (_pending.TryRemove(clOrdID, out var removedTcs))
                {
                    removedTcs.TrySetCanceled();
                }
            });

            return tcs.Task;
        }

        public void CompleteExecutionReport(string clOrdID, OrderResponse response)
        {
            if (_pending.TryRemove(clOrdID, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }
    }
}