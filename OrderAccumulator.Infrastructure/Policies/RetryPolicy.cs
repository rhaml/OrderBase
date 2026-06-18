using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Infrastructure.Policies
{
    public static class RetryPolicy
    {
        public static AsyncRetryPolicy Create()
        {
            return Policy.Handle<DbUpdateConcurrencyException>().WaitAndRetryAsync(retryCount: 5, sleepDurationProvider: retry => TimeSpan.FromMicroseconds(50 * retry));
        }
    }
}
