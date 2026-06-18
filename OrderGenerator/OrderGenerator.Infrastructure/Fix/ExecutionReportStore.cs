using OrderGenerator.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.Infrastructure.Fix
{
    public  class ExecutionReportStore
    {
        public readonly ConcurrentDictionary<string, TaskCompletionSource<ExecutionResult>> _store = new();

        public Task<ExecutionResult>WaitAsync(string clOrdId, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<ExecutionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            _store.TryAdd(clOrdId, tcs);

            cancellationToken.Register(() =>
            {
                if (_store.TryRemove(clOrdId, out var removedTcs))
                {
                    removedTcs.TrySetCanceled();
                }
            });
            return tcs.Task;
        }

        public void Complete(ExecutionResult result)
        {
            if (_store.TryRemove(result.ClOrdId, out var tcs))
            {
                tcs.TrySetResult(result);
            }
        }
    }
}
