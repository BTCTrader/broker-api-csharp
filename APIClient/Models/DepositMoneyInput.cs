using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class DepositMoneyInput
    {
        [JsonProperty("amount")]
        public uint Amount { get; set; }

        [JsonProperty("amount_precision")]
        public uint AmountPrecision { get; set; }
    }
}
