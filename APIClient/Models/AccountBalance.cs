using Newtonsoft.Json;

namespace BTCTrader.APIClient.Models
{
    public class AccountBalance
    {
        [JsonProperty("try_balance")]
        public decimal TRYBalance { get; set; }

        [JsonProperty("btc_balance")]
        public decimal BTCBalance { get; set; }

        [JsonProperty("eth_balance")]
        public decimal ETHBalance { get; set; }

        [JsonProperty("try_reserved")]
        public decimal TRYReserved { get; set; }

        [JsonProperty("btc_reserved")]
        public decimal BTCReserved { get; set; }

        [JsonProperty("eth_reserved")]
        public decimal ETHReserved { get; set; }

        [JsonProperty("try_available")]
        public decimal TRYAvailable { get; set; }

        [JsonProperty("btc_available")]
        public decimal BTCAvailable { get; set; }

        [JsonProperty("eth_available")]
        public decimal ETHAvailable { get; set; }

        [JsonProperty("btctry_taker_fee_percentage")]
        public decimal BTCTRYTakerFeePercentage { get; set; }

        [JsonProperty("btctry_maker_fee_percentage")]
        public decimal BTCTRYMakerFeePercentage { get; set; }

        [JsonProperty("ethtry_taker_fee_percentage")]
        public decimal ETHTRYTakerFeePercentage { get; set; }

        [JsonProperty("ethtry_maker_fee_percentage")]
        public decimal ETHTRYMakerFeePercentage { get; set; }

        [JsonProperty("ethbtc_taker_fee_percentage")]
        public decimal ETHBTCTakerFeePercentage { get; set; }

        [JsonProperty("ethbtc_maker_fee_percentage")]
        public decimal ETHBTCMakerFeePercentage { get; set; }
    }
}
