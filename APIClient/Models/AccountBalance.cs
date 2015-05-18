using System;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class AccountBalance
    {
        [JsonProperty("money_balance")]
        public decimal MoneyBalance { get; set; }

        [JsonProperty("bitcoin_balance")]
        public decimal BitcoinBalance { get; set; }

        [JsonProperty("money_reserved")]
        public decimal MoneyReserved { get; set; }

        [JsonProperty("bitcoin_reserved")]
        public decimal BitcoinReserved { get; set; }

        [JsonProperty("money_available")]
        public decimal MoneyAvailable { get; set; }

        [JsonProperty("bitcoin_available")]
        public decimal BitcoinAvailable { get; set; }

        [JsonProperty("fee_percentage")]
        public decimal FeePercentage { get; set; }

        public override String ToString()
        {
            return "Money Balance: " + MoneyBalance + "\n" + "Bitcoin Balance: " + BitcoinBalance + "\n" +
                   "Money Reserved: " + MoneyReserved + "\n" + "Bitcoin Reserved: " + BitcoinReserved;
        }
    }
}
