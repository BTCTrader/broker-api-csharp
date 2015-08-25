using System;
using BTCTrader.APIClient;
using BTCTrader.APIClient.Models;

namespace BTCTrader.APIExample
{
    class Program
    {
        static void Main() // API Test
        {
            // Alternatively you can set configure these in your web.config or app.config and use the parameterless constructor
            var client = new ApiClient("yourpublicley", "yourprivatekey", "https://www.btcturk.com");

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
            Console.WriteLine("My total money: " + accountBalance.MoneyBalance);

            var openorOrders = client.GetOpenOrders();
            if (openorOrders.Count != 0)
            {
                Console.WriteLine("I have some open orders");
                client.CancelOrder(openorOrders[0]); // Cancel one of my open orders
            }
            else
            {
                Console.WriteLine("I don't have any open orders");
            }

            var trades = client.GetLastTrades(50);
            foreach (var trade in trades)
            {
                Console.WriteLine(trade);
            }

            var depositMoney = client.GetDepositMoney();

            if(depositMoney != null) 
                PrintDepositMoney(depositMoney);

            var depositModel = new DepositMoneyInput
            {
                Amount = 14,
                AmountPrecision = 0
            };

            depositMoney = client.DepositMoney(depositModel);

            if (depositMoney != null)
                PrintDepositMoney(depositMoney);

            var withdrawalMoney = client.GetWithdrawalMoney();

            if (withdrawalMoney != null)
                PrintWithdrawalMoney(withdrawalMoney);

            var withdrawalModel = new WithdrawalMoneyInput
            {
                Amount = 15,
                AmountPrecision = 0,
                BankId = "51c41f6349ede8108423f00a",
                BankName = "AKBANK T.A.S.",
                FriendlyNameSave = true,
                FriendlyName = "Test2",
                Iban = "iban_number_here"
            };

            withdrawalMoney = client.WithdrawalMoney(withdrawalModel);

            if (withdrawalMoney != null)
                PrintWithdrawalMoney(withdrawalMoney);

            var cancelOperation = client.CancelOperation("money_requestid_here");

            Console.WriteLine(cancelOperation);

            //// Submit an ask order at 1,000,000 per btc.
            //var order = new Order
            //{
            //    Price = 657m,
            //    Amount = 1000000m,
            //    Type = Order.SellOrder,
            //    IsMarketOrder = 0
            //};

            //if (client.SubmitOrder(ref order))
            //    Console.WriteLine("Order Id: " + order.Id); // Print the reference order ID to console.

            Console.ReadLine();
        }

        private static void PrintDepositMoney(DepositMoneyOutput output)
        {
            if (string.IsNullOrEmpty(output.DepositCode)) return;
            
            Console.WriteLine(output.Id);
            Console.WriteLine(output.Amount);
            Console.WriteLine(output.AccountOwner);
            if (output.Banks != null)
                Console.WriteLine(output.Banks.Count);
            Console.WriteLine(output.CurrencyType);
            Console.WriteLine(output.DepositCode);
            Console.WriteLine(output.FirstName);
            Console.WriteLine(output.LastName);
        }

        private static void PrintWithdrawalMoney(WithdrawalMoneyOutput output)
        {
            if (output.HasBalanceRequest)
            {
                Console.WriteLine(output.BalanceRequestId);
                Console.WriteLine(output.Amount);

                Console.WriteLine(output.BankName);
                Console.WriteLine(output.HasBalanceRequest);
                Console.WriteLine(output.Iban);
            }
            else
            {
                if (output.BankList != null)
                    foreach (var bank in output.BankList)
                    {
                        Console.WriteLine(bank.Key);
                        Console.WriteLine(bank.Value);
                        Console.WriteLine("--------------------");
                    }

                if (output.FriendlyNameList != null)
                    foreach (var bank in output.FriendlyNameList)
                    {
                        Console.WriteLine(bank.Key);
                        Console.WriteLine(bank.Value);
                        Console.WriteLine("--------------------");
                    }   
            }
        }
    }
}