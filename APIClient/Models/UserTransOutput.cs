using System;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class UserTransOutput
    {
        [JsonProperty("id")]
        public string Id { get; set; } // objectId

        [JsonProperty("date")]
        public DateTime Date { get; set; } // CreatedDate

        [JsonProperty("operation")]
        public string Operation { get; set; } // Operation name

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("funds")]
        public decimal Funds { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
