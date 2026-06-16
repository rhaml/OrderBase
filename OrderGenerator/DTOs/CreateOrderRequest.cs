namespace OrderGenerator.DTOs
{
    public class CreateOrderRequest
    {
        public string Symbol { get; set; }
        public string Side { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
