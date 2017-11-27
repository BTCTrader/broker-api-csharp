namespace BTCTrader.APIClient.Models
{
    public class Order
    {
        public const string Bid = "BuyBtc";
        public const string Ask = "SellBtc";
        public string Id { get; set; }
        public int OrderMethod { get; set; }
        public string Price { get; set; }
        public string PricePrecision { get; set; }
        public string Amount { get; set; }
        public string AmountPrecision { get; set; }
        public string Total { get; set; }
        public string TotalPrecision { get; set; }
        public decimal DenominatorPrecision { get; set; }
        public string TriggerPrice { get; set; }
        public string TriggerPricePrecision { get; set; }
        public int OrderType { get; set; }
        public string PairSymbol { get; set; }
        public string DateTime { get; set; }

        public override string ToString()
        {
            var direction = this.OrderType == 0 ? "Bid" : "Ask";
            var type = "Market";

            switch (OrderMethod)
            {
                case 0: type = "Limit";break;
                case 1: type = "Market";break;
                case 2: type = "Stop Limit";break;
                case 3: type = "Stop Market";break;
            }

            var result = "ID: " + this.Id+"\n";
            result += "Price: " + this.Price + "\n";
            result += "Amount " + this.Amount + "\n";
            result += "Direction: " + direction + "\n";
            result += "Type " + type;

            return result;
        }
    }
}
