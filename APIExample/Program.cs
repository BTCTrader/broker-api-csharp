﻿using System;
using System.Globalization;
using BTCTrader.APIClient;
using BTCTrader.APIClient.Models;

namespace BTCTrader.APIExample
{
    class Program
    {
        static void Main() // API Test
        {
            // Alternatively you can set configure these in your web.config or app.config and use the parameterless constructor
            var client = new ApiClient("pubkey", "privkey", "https://www.btcturk.com/");

            var ticker = client.GetTicker();
            Console.WriteLine(ticker.ToString()); // Print the ticker to console

            var orderbook = client.GetOrderBook();

            var bestBidPrice = orderbook.Bids[0][0];
            var bestBidAmount = orderbook.Bids[0][1];
            Console.WriteLine("Best bid price:" + bestBidPrice); // Print the best bid price and amount to console
            Console.WriteLine("Best bid amount:" + bestBidAmount);

            var bestAskPrice = orderbook.Asks[0][0];
            var bestAskAmount = orderbook.Asks[0][1];
            Console.WriteLine("Best ask price:" + bestAskPrice); // Print the best ask price and amount to console
            Console.WriteLine("Best ask amount:" + bestAskAmount);

            // BELOW THIS LINE REQUIRES AUTHENTICATION
            var accountBalance = client.GetAccountBalance();
            Console.WriteLine("My total Bitcoin: " + accountBalance.BitcoinBalance); // Print my bitcoin balance to console

            var openorOrders = client.GetOpenOrders();
            if (openorOrders.Count != 0)
            {
                Console.WriteLine("I have some open orders");
                client.CancelOrder(openorOrders[0]); // Cancel one of my open orders
            }

            // Submit an ask order at 1,000,000 per btc.
            var order = new Order
            {
                Price = 657m,
                Amount = 1000000m,
                Type = Order.SellOrder,
                IsMarketOrder = 0
            };

            if (client.SubmitOrder(ref order))
                Console.WriteLine("Order Id: " + order.Id); // Print the reference order ID to console.

            Console.ReadLine();
        }
    }
}