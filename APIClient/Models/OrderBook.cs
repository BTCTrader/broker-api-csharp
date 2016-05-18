using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        /// <summary>
        /// Returns the second best bid price on the order book
        /// </summary>
        public decimal GetSecondBestBidPrice()
        {
            var result = decimal.Parse(Bids[1][0].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
            return result;
        }

        /// <summary>
        /// Returns the second best ask price on the order book
        /// </summary>
        public decimal GetSecondBestAskPrice()
        {
            var result = decimal.Parse(Asks[1][0].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
            return result;
        }

        /// <summary>
        /// Returns the sum of amounts of bids above the given price.
        /// </summary>
        public decimal GetSumOfBidsAbove(decimal price)
        {
            var bidsGreaterThanThePrice = Bids.Where(bidOrder => bidOrder[0] >= price);
            return bidsGreaterThanThePrice.Aggregate<dynamic, decimal>(0, (current, bidOrder) => (decimal)(current + decimal.Parse(bidOrder[1].ToString(), new NumberFormatInfo { NumberDecimalSeparator = "." })));
        }

        /// <summary>
        /// Returns the sum of amounts of asks below the given price.
        /// </summary>
        public decimal GetSumOfAsksBelow(decimal price)
        {
            var asksLessThanPrice = Asks.Where(askOrder => askOrder[0] <= price);
            return asksLessThanPrice.Aggregate<dynamic, decimal>(0, (current, askOrder) => (decimal)(current + decimal.Parse(askOrder[1].ToString(), new NumberFormatInfo { NumberDecimalSeparator = "." })));
        }

        /// <summary>
        /// Returns the price after counting up the given amount on the bids
        /// Can be used to calculate what the bid price will become after the given amount is bought.
        /// </summary>
        public decimal GetBidPriceAfter(decimal amount)
        {
            decimal sum = 0;
            decimal price = 0;
            foreach (var order in Bids)
            {
                sum += decimal.Parse(order[1].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
                if (sum > amount)
                {
                    price = decimal.Parse(order[0].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
                    break;
                }
            }
            return price;
        }

        /// <summary>
        /// Returns the price after counting up the given amount on the asks
        /// Can be used to calculate what the ask price will become after the given amount is sold.
        /// </summary>
        public decimal GetAskPriceAfter(decimal amount)
        {
            decimal sum = 0;
            decimal price = 0;
            foreach (var order in Asks)
            {
                sum += decimal.Parse(order[1].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
                if (sum > amount)
                {
                    price = decimal.Parse(order[0].ToString(CultureInfo.InvariantCulture), new NumberFormatInfo { NumberDecimalSeparator = "." });
                    break;
                }
            }
            return price;
        }
    }
}
