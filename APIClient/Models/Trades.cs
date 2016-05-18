using System;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class Trades
    {
        [JsonProperty("date")]
        public double Date { get; set; }
        [JsonProperty("tid")]
        public string Tid { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return "Date: " + Date + "\n" + "Tid: " + Tid + "\n" +
                "Price: " + Price + "\n" + "Amount: " + Amount;
        }
    }
}
