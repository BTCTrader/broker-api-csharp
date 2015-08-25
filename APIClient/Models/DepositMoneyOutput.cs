using System.Collections.Generic;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class DepositMoneyOutput
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("deposit_code")]
        public string DepositCode { get; set; }

        [JsonProperty("banks")]
        public IList<BankAccount> Banks { get; set; }

        [JsonProperty("currency_type")]
        public string CurrencyType { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("account_owner")]
        public string AccountOwner { get; set; }
    }
}
