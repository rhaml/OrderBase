using FluentAssertions;
using OrderGenerator.Infrastructure.Fix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderGenerator.UnitTests
{
    public class FixGatewayTests
    {
        [Fact]
        public async Task Should_Timeout_When_Report_Not_Received()
        {
            var store = new ExecutionReportStore();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            var act = async () => await store.WaitAsync("abc", cts.Token);
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}
