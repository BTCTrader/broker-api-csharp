using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class BankAccount
    {
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("iban")]
        public string Iban { get; set; }
    }
}
