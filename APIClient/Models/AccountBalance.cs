using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class AccountBalance
    {
        [JsonProperty("denominator_balance")]
        public decimal DenominatorBalance { get; set; }

        [JsonProperty("numerator_balance")]
        public decimal NumeratorBalance { get; set; }

        [JsonProperty("denominator_reserved")]
        public decimal DenominatorReserved { get; set; }

        [JsonProperty("numerator_reserved")]
        public decimal NumeratorReserved { get; set; }

        [JsonProperty("denominator_available")]
        public decimal DenominatorAvailable { get; set; }

        [JsonProperty("numerator_available")]
        public decimal NumeratorAvailable { get; set; }

        [JsonProperty("fee_percentage")]
        public decimal FeePercentage { get; set; }

        public string PairSymbol { get; set; }
        public override string ToString()
        {
            return "Denominator Balance: " + DenominatorBalance + "\n" + "Numerator Balance: " + NumeratorBalance + "\n" +
                   "Denominator Reserved: " + DenominatorReserved + "\n" + "Numerator Reserved: " + NumeratorReserved + "\n" +
                   "Pair Symbol: " + PairSymbol;
        }
    }
}
