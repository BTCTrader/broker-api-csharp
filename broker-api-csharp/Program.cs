using System;
using System.Linq;

namespace broker_api_csharp
{
    class Program
    {
        static void Main()
        {
            // API Test
            var client = new ApiClient("55507012a7b7211964846e79", "9fVzdZmxy4u+1LJ3SsfiYBsZI4+S+FpM",
                "https://btctrader-broker-acntrade.azurewebsites.net/");

            client.GetOrderBook();

            client.UpdateAccountBalance();

            var order = new ApiClient.Order
            {
                Price = 1000m,
                Amount = 1000001m,
                Type = ApiClient.Order.SellOrder,
            };

            client.SubmitOrder(order);

            Console.ReadLine();
        }
    }
}