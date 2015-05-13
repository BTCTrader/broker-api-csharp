using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace broker_api_csharp
{
    public class ApiClient : IApiClient
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _baseUrl;

        public Ticker ApiTicker { get; set; }

        public AccountBalance Balance { get; set; }

        public ApiClient(string publicKey, string privateKey, string baseUrl)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;
            _baseUrl = baseUrl;
        }

        public ApiClient()
        {
            _privateKey = ConfigurationManager.ConnectionStrings["PrivateApiKey"].ConnectionString;
            _publicKey = ConfigurationManager.ConnectionStrings["PublicApiKey"].ConnectionString;
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        }

        /// <summary>
        /// Sets request headers and base uri and returns an HTTP client which can be used to make authenticated requests.
        /// </summary>
        /// <returns>HttpClient with necessary authentication headers set</returns>
        private HttpClient SetAuthencation()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            client.DefaultRequestHeaders.Add("X-PCK", _publicKey);
            var stamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
            client.DefaultRequestHeaders.Add("X-Stamp", stamp.ToString(CultureInfo.InvariantCulture));
            var data = String.Format("{0}{1}", _publicKey, stamp);
            using (var hmac = new HMACSHA256(Convert.FromBase64String(_privateKey)))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                var signature = Convert.ToBase64String(signatureBytes);
                client.DefaultRequestHeaders.Add("X-Signature", signature);
            }
            return client;
        }

        /// <summary>
        /// Converts a Datatime to an equivalent Unix Timestamp, in seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// Submits given Order. Requires authentication.
        /// </summary>
        /// <param name="order">Order to be submitted</param>
        /// <returns>True if Order is submitted successfully, false if it was not.</returns>
        public bool SubmitOrder(Order order)
        {
            var client = SetAuthencation();
            var method = order.Type == Order.BuyOrder ? "api/buey" : "api/seall";
            order.Price = Math.Round(order.Price, 2);

            var response = client.PostAsJsonAsync(method, order).Result;

            return RequestSucceeded(response);
        }

        /// <summary>
        /// Sells all available bitcoin in user account with market order
        /// </summary>
        /// <returns>True if order submitted successfully, false if order submission failed</returns>
        public bool SellAllMyBitcoin()
        {
            var order = new Order
            {
                IsMarketOrder = 1,
                Type = Order.SellOrder,
                Amount = Balance.BitcoinAvailable,
            };
            return SubmitOrder(order);
        }

        /// <summary>
        /// Buy bitcoin with all available MoneyBalance in user account with market order
        /// </summary>
        /// <returns>True if order submitted successfully, false if order submission failed</returns>
        public bool BuyWithAllMyMoney()
        {
            var order = new Order
            {
                IsMarketOrder = 1,
                Type = Order.BuyOrder,
                Total = Balance.MoneyAvailable,
            };
            return SubmitOrder(order);
        }

        /// <summary>
        /// Get the authenticated account's balance
        /// </summary>
        /// <returns>An onject of type AccountBalance. Null if account balance cannot be retreived </returns>
        public bool UpdateAccountBalance()
        {
            var client = SetAuthencation();
            var response = client.GetAsync("api/balance").Result;

            if (RequestSucceeded(response))
            {
                Balance = JsonConvert.DeserializeObject<AccountBalance>(response.Content.ReadAsStringAsync().Result);
                return true;
            }
            return false;
        }

        public IList<UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending)
        {
            var client = SetAuthencation();
            var requestUri = "api/usertransactions?limit=" + limit + "&offset=" + offset;

            if (ascending)
                requestUri += "&sort=asc";
            else
                requestUri += "&sort=desc";

            var response = client.GetAsync(requestUri).Result;
            if (RequestSucceeded(response))
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var transactions = JsonConvert.DeserializeObject<UserTransOutput[]>(content);
                return transactions;
            }
            return null;
        }

        /// <summary>
        /// Cancels order with given OrderId
        /// </summary>
        /// <returns>True if order was cancelled, false otherwise</returns>
        public bool CancelOrder(Order order)
        {
            var client = SetAuthencation();
            var response = client.PostAsJsonAsync("api/cancelOrder", new { id = order.Id }).Result;
            return RequestSucceeded(response);
        }

        /// <summary>
        /// Cancels all open orders of the user
        /// </summary>
        public void CancelAllOpenOrders()
        {
            var orders = GetOpenOrders();

            if (orders == null)
                return;

            foreach (var order in orders)
            {
                CancelOrder(order);
            }
        }

        /// <summary>
        /// Get all open orders of the user
        /// </summary>
        /// <returns>Users open orders listed. Null if there was an error</returns>
        public IList<Order> GetOpenOrders()
        {
            var client = SetAuthencation();
            var response = client.GetAsync("api/openOrders").Result;
            if (RequestSucceeded(response))
            {
                var result = JsonConvert.DeserializeObject<IList<Order>>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Updates the Ticker object
        /// </summary>
        /// <returns>True if the update succeeded, false otherwise</returns>
        public bool UpdateTicker()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            var response = client.GetAsync("api/ticker").Result;
            return RequestSucceeded(response);
        }

        /// <summary>
        /// Get the orderbook
        /// </summary>
        /// <returns>The orderbook. Null if there was an error</returns>
        public OrderBook GetOrderBook()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            var response = client.GetAsync("api/orderbook").Result;
            if (RequestSucceeded(response))
            {
                var result = JsonConvert.DeserializeObject<OrderBook>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Get the last trades in the market
        /// </summary>
        /// <returns>Null if there was an error</returns>
        public void GetLastTrades()
        {
            throw new Exception("Not implemented!");
        }

        /// <summary>
        /// Handles the response received from the application. You can change this method to have custom error handling in your app.
        /// </summary>
        /// <returns>Returns false if there were no errors. True if request failed.</returns>
        private static bool RequestSucceeded(HttpResponseMessage response)
        {
            var requestSucceeded = true;
            var result = response.Content.ReadAsAsync<dynamic>().Result;

            if (!response.IsSuccessStatusCode)
            {
                //TODO: Write your own error handling code.
                Console.WriteLine("Received error. Status code: " + response.StatusCode + ". Error message: " + response.ReasonPhrase);
                requestSucceeded = false;
            }

            if (result["error"] != null)
            {
                //TODO: Write your own error handling code.
                Console.WriteLine("Received error. Status code: " + result["error"]["code"] + ". Error message: " + result["error"]["message"]);
                requestSucceeded = false;
            }

            return requestSucceeded;
        }

        public class Order
        {
            public const string BuyOrder = "BuyBtc";
            public const string SellOrder = "SellBtc";
            public string Id { get; set; }
            public int IsMarketOrder { get; set; }
            public decimal Price { get; set; }
            public decimal Amount { get; set; }
            public decimal Total { get; set; }
            public string Type { get; set; }
        }

        public class AccountBalance
        {
            [JsonProperty("money_balance")]
            public decimal MoneyBalance { get; set; }

            [JsonProperty("bitcoin_balance")]
            public decimal BitcoinBalance { get; set; }

            [JsonProperty("money_reserved")]
            public decimal MoneyReserved { get; set; }

            [JsonProperty("bitcoin_reserved")]
            public decimal BitcoinReserved { get; set; }

            [JsonProperty("money_available")]
            public decimal MoneyAvailable { get; set; }

            [JsonProperty("bitcoin_available")]
            public decimal BitcoinAvailable { get; set; }

            [JsonProperty("fee_percentage")]
            public decimal FeePercentage { get; set; }

            public override String ToString()
            {
                return "Money Balance: " + MoneyBalance + "\n" + "Bitcoin Balance: " + BitcoinBalance + "\n" +
                       "Money Reserved: " + MoneyReserved + "\n" + "Bitcoin Reserved: " + BitcoinReserved;
            }
        }

        public class Ticker
        {
            public decimal Last { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Volume { get; set; }
            public decimal Bid { get; set; }
            public decimal Ask { get; set; }
            public override String ToString()
            {
                return "Last: " + Last + "\n" + "High: " + High + "\n" +
                       "Low: " + Low + "\n" + "Volume: " + Volume + "\n" +
                       "Bid: " + Bid + "\n" + "Ask: " + Ask;
            }
        }

        public class OrderBook
        {
            [JsonProperty("timeStamp")]
            public decimal TimeStamp { get; set; }

            [JsonProperty("bids")]
            public IList<dynamic> Bids { get; set; }

            [JsonProperty("asks")]
            public IList<dynamic> Asks { get; set; }
        }

        public class UserTransInput
        {
            public string Sort { get; set; } // sorting by desc, asc
            public int Offset { get; set; } // number of skiping records
            public int Limit { get; set; } // number of limit query
        }

        public class UserTransOutput
        {
            [JsonProperty("Id")]
            public string Id { get; set; } // objectId

            [JsonProperty("Date")]
            public DateTime Date { get; set; } // CreatedDate

            [JsonProperty("Operation")]
            public string Operation { get; set; } // Operation name

            [JsonProperty("Btc")]
            public decimal Btc
            {
                get;
                set;
            }

            [JsonProperty("Currency")]
            public decimal Currency { get; set; }

            [JsonProperty("Price")]
            public decimal Price { get; set; }
        }
    }
}