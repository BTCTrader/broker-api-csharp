using System;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class UserTransOutput
    {
        [JsonProperty("Id")]
        public string Id { get; set; } // objectId

        [JsonProperty("Date")]
        public DateTime Date { get; set; } // CreatedDate

        [JsonProperty("Operation")]
        public string Operation { get; set; } // Operation name

        [JsonProperty("Numerator")]
        public decimal Numerator{get;set;}

        [JsonProperty("Denominator")]
        public decimal Denominator { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }
    }
}
