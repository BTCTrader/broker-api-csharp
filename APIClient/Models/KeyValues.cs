using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class KeyValues
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
