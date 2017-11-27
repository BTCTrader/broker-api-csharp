using System;
using BTCTrader.APIClient;
using BTCTrader.APIClient.Models;

namespace BTCTrader.APIExample
{
    /// <summary>
    /// Shows the usage of the APIClient with various examples.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            // Alternatively you can set configure these in your web.config or app.config and use the parameterless constructor
            var client = new ApiClient("PUBLIC KEY", "PRIVAYE KEY", "https://www.btcturk.com/");

            // Print the ticker to the console
            var ticker = client.GetTicker("BTCTRY");
            Console.WriteLine(ticker.ToString());

            // Print the best bid price and amount to the console
            var orderbook = client.GetOrderBook("BTCTRY");
            var bestBidPrice = orderbook.Bids[0][0];
            var bestBidAmount = orderbook.Bids[0][1];
            Console.WriteLine("Best bid price:" + bestBidPrice);
            Console.WriteLine("Best bid amount:" + bestBidAmount);

            // Print the best ask price and amount to the console
            var bestAskPrice = orderbook.Asks[0][0];
            var bestAskAmount = orderbook.Asks[0][1];
            Console.WriteLine("Best ask price:" + bestAskPrice);
            Console.WriteLine("Best ask amount:" + bestAskAmount);

            // Print the last 10 trades in the market to the console.
            var trades = client.GetLastTrades(10, "BTCTRY");
            Console.WriteLine("Last 10 trades in the market");
            foreach(var trade in trades)
            {
                Console.WriteLine(trade);
            }

            // Print the last 7 days' OHLC to the console
            var ohlc = client.GetDailyOHLC("BTCTRY", 7);
            foreach(var dailyOhlc in ohlc)
            {
                Console.WriteLine(dailyOhlc);
            }

            // BELOW THIS LINE REQUIRES AUTHENTICATION
            var accountBalance = client.GetAccountBalance();
            Console.WriteLine("My total Bitcoin: " + accountBalance.BTCBalance); // Print my bitcoin balance to console
            Console.WriteLine("My total Ethereum: " + accountBalance.ETHBalance);
            Console.WriteLine("My total money: " + accountBalance.TRYBalance);
            var price = ticker.Ask.ToString().Split('.');

            var orderBid = new Order
            {
                Price = price[0],
                Amount = "0",
                AmountPrecision = "001",
                OrderType = 1,
                OrderMethod = 0,
                PairSymbol = "BTCTRY",
                DenominatorPrecision = 2,
                Total = "1000",
                TotalPrecision = "00",
                TriggerPrice = "0",
                TriggerPricePrecision = "00",
                PricePrecision = "00"
            };

            if(client.SubmitOrder(ref orderBid))
            {
                Console.WriteLine("The submitted order was assigned the Id: " + orderBid.Id);
            }

            var orderAsk = new Order
            {
                Price = price[0],
                Amount = "0",
                AmountPrecision = "001",
                OrderType = 1,
                OrderMethod = 1,
                PairSymbol = "BTCTRY",
                DenominatorPrecision = 2,
                Total = "1000",
                TotalPrecision = "00",
                TriggerPrice = "0",
                TriggerPricePrecision = "00",
                PricePrecision = "00"
            };

            if(client.SubmitOrder(ref orderAsk))
            {
                Console.WriteLine("The submitted order was assigned the Id: " + orderAsk.Id);
            }

            var t = client.BuyWithAllMyMoney();

            var openorOrders = client.GetOpenOrders("BTCTRY");
            if(openorOrders.Count != 0)
            {
                Console.WriteLine("I have some open orders");
                client.CancelOrder(openorOrders[0]); // Cancel one of my open orders
            }
            else
            {
                Console.WriteLine("I don't have any open orders");
            }



            Console.ReadLine();
        }

    }
}