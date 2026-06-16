namespace OrderAccumulator.Models
{
    public sealed record ExposureResult(bool Accepted, string Symbol, decimal CurrentExposure, decimal NewExposure, decimal Delta, string? RejectionReason = null);
}
