using FluentAssertions;
using OrderAccumulator.Domain.Entities;
using OrderAccumulator.Domain.Enums;
using OrderAccumulator.Domain.Models;

namespace OrderAccumulator.Domain.Tests;

public class ExposureTests
{
    [Fact]
    public void Should_Accept_Buy_Order()
    {
        var exposure = new Exposure("PETR4");

        var orderRequest = new OrderRequest("PETR4", OrderSide.Buy, 1000, 50);

        var result = exposure.ApplyOrder(orderRequest);

        result.Accepted.Should().BeTrue();
        result.NewExposure.Should().Be(50000);
    }

    [Fact]
    public void Should_Decrease_Exposure_When_Sell()
    {
        var exposure = new Exposure("PETR4", 100000);

        var orderRequest = new OrderRequest("PETR4", OrderSide.Sell, 100, 100);
        var result = exposure.ApplyOrder(orderRequest);
        result.Accepted.Should().BeTrue();
        result.NewExposure.Should().Be(90000);
    }

    [Fact]
    public void Should_Reject_When_Exceeding_Limit()
    {
        var exposure = new Exposure("PETR4", 99999000);
        var orderRequest = new OrderRequest("PETR4", OrderSide.Buy, 100, 100);
        var result = exposure.ApplyOrder(orderRequest);
        result.Accepted.Should().BeFalse();
        result.RejectionReason.Should().Be("Exposure limit exceeded");
    }
}
