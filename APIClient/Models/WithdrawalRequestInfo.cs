using System.Collections.Generic;
using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    /// <summary>
    /// Model for existing withdrawal requests and info model for withdrawal requests
    /// </summary>
    public class WithdrawalRequestInfo
    {
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("bank_list")]
        public IEnumerable<KeyValues> BankList { get; set; }

        [JsonProperty("friendly_name_list")]
        public IEnumerable<KeyValues> FriendlyNameList { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("has_balance_request")]
        public bool HasBalanceRequest { get; set; }

        [JsonProperty("balance_request_id")]
        public string BalanceRequestId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
