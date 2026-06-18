namespace OrderGenerator.Api.Contracts
{
    public class OrderResponse
    { 
        public string ClOrdId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Side { get; set; }
    }
}
