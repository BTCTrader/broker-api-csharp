using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class WithdrawalMoneyInput
    {
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }

        [JsonProperty("friendly_name_save")]
        public bool FriendlyNameSave { get; set; }

        [JsonProperty("amount")]
        public uint Amount { get; set; }

        [JsonProperty("amount_precision")]
        public uint AmountPrecision { get; set; }

        [JsonProperty("has_balance_request")]
        public bool HasBalanceRequest { get; set; }

        [JsonProperty("balance_request_id")]
        public string BalanceRequestId { get; set; }

        [JsonProperty("bank_id")]
        public string BankId { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }
    }
}
