using System;

namespace BTCTrader.APIClient.Models
{
    public class OHLC
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal Average { get; set; }
        public decimal DailyChangeAmount { get; set; }
        public decimal DailyChangePercentage { get; set; }

        public override string ToString()
        {
            var result = "Date: " + Date;
            result += "\n" + "Open: " + Open;
            result += "\n" + "High: " + High;
            result += "\n" + "Low: " + Low;
            result += "\n" + "Volume: " + Volume;
            result += "\n" + "Average: " + Average;
            result += "\n" + "DailyChangeAmount: " + DailyChangeAmount;
            result += "\n" + "DailyChangePercentage: " + DailyChangePercentage;
            return result;
        }
    }
}