namespace broker_api_csharp.Models
{
    public class Order
    {
        public const string BuyOrder = "BuyBtc";
        public const string SellOrder = "SellBtc";
        public string Id { get; set; }
        public int IsMarketOrder { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string Type { get; set; }
        public string DateTime { get; set; }
    }
}
