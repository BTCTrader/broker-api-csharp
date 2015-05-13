﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace broker_api_csharp.Models
{
    public class OrderBook
    {
        [JsonProperty("timeStamp")]
        public decimal TimeStamp { get; set; }

        [JsonProperty("bids")]
        public IList<dynamic> Bids { get; set; }

        [JsonProperty("asks")]
        public IList<dynamic> Asks { get; set; }
    }
}
