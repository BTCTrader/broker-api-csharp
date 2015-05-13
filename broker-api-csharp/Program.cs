using System;
using broker_api_csharp.Models;

namespace broker_api_csharp
{
    class Program
    {
        static void Main()
        {
            // API Test
            var client = new ApiClient("55507012a7b7211964846e79", "9fVzdZmxy4u+1LJ3SsfiYBsZI4+S+FpM",
                "https://btctrader-broker-acntrade.azurewebsites.net/");

            var orders=client.GetOrderBook();

            client.UpdateAccountBalance();

            var myAvaliableBitcoin = client.Balance.BitcoinAvailable;

            var order = new Order
            {
                Price = 1000m,
                Amount = 0.1m,
                Type = Order.SellOrder,
            };

            if(client.SubmitOrder(ref order))
                Console.WriteLine(order.Id);

            Console.ReadLine();
        }
    }
}