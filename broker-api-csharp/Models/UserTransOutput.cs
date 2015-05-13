using System;
using Newtonsoft.Json;

namespace broker_api_csharp.Models
{
    public class UserTransOutput
    {
        [JsonProperty("Id")]
        public string Id { get; set; } // objectId

        [JsonProperty("Date")]
        public DateTime Date { get; set; } // CreatedDate

        [JsonProperty("Operation")]
        public string Operation { get; set; } // Operation name

        [JsonProperty("Btc")]
        public decimal Btc{get;set;}

        [JsonProperty("Currency")]
        public decimal Currency { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }
    }
}
