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
            var client = new ApiClient("PUBLIC KEY", "PRIVATE KEY", "https://www.btcturk.com/");

            // Print the ticker to the console
            var ticker = client.GetTicker();
            Console.WriteLine(ticker.ToString());

            // Print the best bid price and amount to the console
            var orderbook = client.GetOrderBook();
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
            var trades = client.GetLastTrades(10);
            Console.WriteLine("Last 10 trades in the market");
            foreach (var trade in trades)
            {
                Console.WriteLine(trade);
            }

            // Print the last 7 days' OHLC to the console
            var ohlc = client.GetDailyOHLC(7);
            foreach (var dailyOhlc in ohlc)
            {
                Console.WriteLine(dailyOhlc);
            }

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

            //Deposit Money
            //GET
            var depositRequestResult = client.GetDepositRequest();
            if (depositRequestResult != null)
            {
                PrintDepositMoney(depositRequestResult);
            }

            //Deposit Money
            //POST
            var depositRequest = new DepositRequest
            {
                Amount = "14",
                AmountPrecision = "0"
            };

            depositRequestResult = client.MakeDepositRequest(depositRequest);
            if (depositRequestResult != null)
            {
                PrintDepositMoney(depositRequestResult);
            }

            //Withdrawal Money
            //GET
            var existingWithdrawalRequest = client.GetWithdrawalRequest();
            if (existingWithdrawalRequest != null)
            {
                PrintWithdrawalMoney(existingWithdrawalRequest);
            }

            //Withdrawal Money
            //POST
            var withdrawalRequest = new WithdrawalRequest
            {
                Amount = "15",
                AmountPrecision = "0",
                BankId = "51c41f6349ede8108423f00a",
                BankName = "AKBANK T.A.S.",
                FriendlyNameSave = true,
                FriendlyName = "Test2",
                Iban = "Iban_Number_Here"
            };

            existingWithdrawalRequest = client.MakeWithdrawalRequest(withdrawalRequest);
            if (existingWithdrawalRequest != null)
            {
                PrintWithdrawalMoney(existingWithdrawalRequest);
            }

            //Deposit Money
            //DELETE
            var cancelOperation = client.CancelDepositRequest("balance_request_id_here");

            Console.WriteLine(cancelOperation);

            //Withdrawal Money
            //DELETE
            cancelOperation = client.CancelWithdrawalRequest("balance_request_id_here");
            Console.WriteLine(cancelOperation);

            // Submit an ask order at 1,000,000 per btc and print the received order ID to the console.
            var order = new Order
            {
                Price = 657m,
                Amount = 1000000m,
                Type = Order.SellOrder,
                IsMarketOrder = 0
            };
            
            if (client.SubmitOrder(ref order))
            {
                Console.WriteLine("The submitted order was assigned the Id: " + order.Id); 
            }

            Console.ReadLine();
        }

        private static void PrintDepositMoney(DepositRequestResult output)
        {
            if (string.IsNullOrEmpty(output.DepositCode)) return;

            Console.WriteLine(output.Id);
            Console.WriteLine(output.Amount);
            Console.WriteLine(output.AccountOwner);

            foreach (var bankAccount in output.Banks)
            {
                Console.WriteLine(bankAccount.BankName);
                Console.WriteLine(bankAccount.Iban);
            }

            Console.WriteLine(output.CurrencyType);
            Console.WriteLine(output.DepositCode);
            Console.WriteLine(output.FirstName);
            Console.WriteLine(output.LastName);
        }

        private static void PrintWithdrawalMoney(WithdrawalRequestInfo output)
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