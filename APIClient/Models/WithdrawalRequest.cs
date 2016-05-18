using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class WithdrawalRequest
    {
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }

        [JsonProperty("friendly_name_save")]
        public bool FriendlyNameSave { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("amount_precision")]
        public string AmountPrecision { get; set; }

        [JsonProperty("has_balance_request")]
        public bool HasBalanceRequest { get; set; }

        [JsonProperty("balance_request_id")]
        public string BalanceRequestId { get; set; }

        [JsonProperty("bank_id")]
        public string BankId { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
