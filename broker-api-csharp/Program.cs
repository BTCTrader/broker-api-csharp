using System;

namespace broker_api_csharp
{
    class Program
    {
        static void Main() // API Test
        {
            // Alternatively you can set configure these in your web.config or app.config and use the parameterless constructor
            var client = new ApiClient("yourpublickey", "yourprivatekey", "https://btctrader-broker-btcturk.azurewebsites.net");
            
            //var ticker = client.GetTicker();
            //var orderbook = client.GetOrderBook();
            //var accountBalance = client.GetAccountBalance();
            //var openorOrders = client.GetOpenOrders();
            
            // Submit an ask order at 1,000,000 per btc.
            var order = new ApiClient.Order
            {
                Price = 1000000m,
                Amount = 100000001m,
                Type = ApiClient.Order.SellOrder,
                IsMarketOrder = 0
            };

            client.SubmitOrder(order);

            Console.ReadLine();
        }
    }
}