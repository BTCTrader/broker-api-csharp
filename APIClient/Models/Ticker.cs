using System;

namespace BTCTrader.APIClient.Models
{
    public class Ticker
    {
        public decimal Last { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Open { get; set; }
        public override String ToString()
        {
            return "Last: " + Last + "\n" + "High: " + High + "\n" +
                   "Low: " + Low + "\n" + "Volume: " + Volume + "\n" +
                   "Bid: " + Bid + "\n" + "Ask: " + Ask + "\n" +
                   "Open: " + Open;
        }
    }

}
