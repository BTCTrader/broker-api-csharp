﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class OrderBook
    {
        [JsonProperty("timeStamp")]
        public decimal TimeStamp { get; set; }

        [JsonProperty("bids")]
        public IList<IList<decimal>> Bids { get; set; }

        [JsonProperty("asks")]
        public IList<IList<decimal>> Asks { get; set; }
    }
}
