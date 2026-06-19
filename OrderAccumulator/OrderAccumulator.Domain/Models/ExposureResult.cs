namespace OrderAccumulator.Models
{
    public record ExposureResult(bool Accepted, 
        string Symbol, 
        decimal CurrentExposure, 
        decimal NewExposure, 
        decimal Delta, 
        string? RejectionReason = null);
}
