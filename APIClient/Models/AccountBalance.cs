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

        [JsonProperty("btctry_fee_percentage")]
        public decimal BTCTRYFeePercentage { get; set; }

        [JsonProperty("btctry_maker_fee_percentage")]
        public decimal BTCTRYMakerFeePercentage { get; set; }

        [JsonProperty("ethtry_fee_percentage")]
        public decimal ETHTRYFeePercentage { get; set; }

        [JsonProperty("ethtry_maker_fee_percentage")]
        public decimal ETHTRYMakerFeePercentage { get; set; }

        [JsonProperty("ethbtc_fee_percentage")]
        public decimal ETHBTCFeePercentage { get; set; }

        [JsonProperty("ethbtc_maker_fee_percentage")]
        public decimal ETHBTCMakerFeePercentage { get; set; }

        public override string ToString()
        {
            return "TRY Balance: " + TRYBalance + "\n" + "BTC Balance: " + BTCBalance + "\n" + "ETH Balance: "
                   + ETHBalance + "\n" + "TRY Reserved: " + TRYReserved + "\n" + "BTC Reserved: " + BTCReserved + "\n"
                   + "ETH Reserved: " + ETHReserved + "\n";
        }
    }
}
