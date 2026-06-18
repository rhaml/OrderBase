namespace OrderGenerator.Api.Contracts
{
    public class CreateOrderRequest
    {
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
