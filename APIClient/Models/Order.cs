namespace BTCTrader.APIClient.Models
{
    public class Order
    {
        public const string Bid = "BuyBtc";
        public const string Ask = "SellBtc";
        public string Id { get; set; }
        public int IsMarketOrder { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string Type { get; set; }
        public string DateTime { get; set; }

        public override string ToString()
        {
            var direction = this.Type == Bid ? "Bid" : "Ask";
            var type = this.IsMarketOrder == 1 ? "Market" : "Limit";

            var result = "ID: " + this.Id+"\n";
            result += "Price: " + this.Price + "\n";
            result += "Amount " + this.Amount + "\n";
            result += "Direction: " + direction + "\n";
            result += "Type " + type;

            return result;
        }
    }
}
