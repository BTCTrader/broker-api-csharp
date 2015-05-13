using System;
using broker_api_csharp.Models;

namespace broker_api_csharp
{
    class Program
    {
        static void Main() // API Test
        {
            // Alternatively you can set configure these in your web.config or app.config and use the parameterless constructor
            var client = new ApiClient("yourpublickey", "yourprivatekey", "https://btctrader-broker-btcturk.azurewebsites.net");

            var ticker = client.GetTicker();
            var orderbook = client.GetOrderBook();
            var accountBalance = client.GetAccountBalance();
            var openorOrders = client.GetOpenOrders();
            
            // Submit an ask order at 1,000,000 per btc.
            var order = new Order
            {
                Price = 657m,
                Amount = 1m,
                Type = Order.SellOrder,
                IsMarketOrder = 0
            };

            if(client.SubmitOrder(ref order))
                Console.WriteLine("Order Id: " + order.Id);

            Console.ReadLine();
        }
    }
}