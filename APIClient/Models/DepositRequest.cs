using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class DepositRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("amount_precision")]
        public string AmountPrecision { get; set; }
    }
}
