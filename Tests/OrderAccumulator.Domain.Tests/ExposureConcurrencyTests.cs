using FluentAssertions;
using OrderAccumulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAccumulator.Domain.Tests
{
    public class ExposureConcurrencyTests
    {
        [Fact]
        public async Task Should_Handle_1000_Orders()
        {
            var exposure = new Exposure("PETR4");
            var tasks  = Enumerable.Range(0, 1000).Select(_ => 
                    Task.Run(() =>
                    {
                        exposure.ApplyOrder(new Models.OrderRequest("PETR4", Enums.OrderSide.Buy, 1, 10));
                    })
                );

            await Task.WhenAll(tasks);
            exposure.Value.Should().Be(10000);
        }
    }
}
