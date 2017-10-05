namespace BTCTrader.APIClient.Models
{
    public class Order
    {
        public const string Bid = "Buy";
        public const string Ask = "Sell";
        public string Id { get; set; }
        public int IsMarketOrder { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string Type { get; set; }
        public string PairSymbol { get; set; }
        public string DateTime { get; set; }

        public override string ToString()
        {
            var direction = Type == Bid ? "Bid" : "Ask";
            var type = IsMarketOrder == 1 ? "Market" : "Limit";

            var result = "ID: " + Id+"\n";
            result += "Price: " + Price + "\n";
            result += "Amount " + Amount + "\n";
            result += "PairSymbol " + PairSymbol + "\n";
            result += "Direction: " + direction + "\n";
            result += "Type " + type;

            return result;
        }
    }
}
