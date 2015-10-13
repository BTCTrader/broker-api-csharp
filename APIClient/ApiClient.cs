using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using BTCTrader.APIClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BTCTrader.APIClient
{
    public class ApiClient : IApiClient
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _baseUrl;

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
        /// <returns>HttpClient with necessary authentication headers set.</returns>
        private HttpResponseMessage SendRequest<T>(HttpVerbs method, string requestUri, T value, bool requireAuthenticate)
        {
            HttpResponseMessage response = null;

            using (var client = new HttpClient { BaseAddress = new Uri(_baseUrl), Timeout = TimeSpan.FromSeconds(30) })
            {
                if (requireAuthenticate)
                {
                    client.DefaultRequestHeaders.Add("X-PCK", _publicKey);
                    var stamp = GetStamp();
                    client.DefaultRequestHeaders.Add("X-Stamp", stamp.ToString(CultureInfo.InvariantCulture));
                    var signature = GetSignature(stamp);
                    client.DefaultRequestHeaders.Add("X-Signature", signature);
                }

                try
                {
                    switch (method)
                    {
                        case HttpVerbs.Post:
                            response = client.PostAsJsonAsync(requestUri, value).Result;
                            break;
                        case HttpVerbs.Get:
                            response = client.GetAsync(requestUri).Result;
                            break;
                        case HttpVerbs.Delete:
                            response = client.DeleteAsync(requestUri).Result;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    response = null;
                }

                if (response != null)
                {
                    if (!RequestSucceeded(response))
                        response = null;
                }
            }

            return response;
        }

        private static long GetStamp()
        {
            var stamp = DateTime.UtcNow.Ticks;

            return stamp;
        }

        private string GetSignature(long stamp)
        {
            String signature = null;
            try
            {
                var data = String.Format("{0}{1}", _publicKey, stamp);
                using (var hmac = new HMACSHA256(Convert.FromBase64String(_privateKey)))
                {
                    var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    signature = Convert.ToBase64String(signatureBytes);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception occured in GetSignature method. The likely cause is a private or public key in wrong format. Exception:" + e.Message);
            }

            return signature;
        }

        /// <summary>
        /// Submits given Order. Requires authentication.
        /// </summary>
        /// <param name="order">Order to be submitted</param>
        /// <returns>True if Order is submitted successfully, false if it was not.</returns>
        private bool SubmitOrder(ref Order order)
        {
            var result = false;
            var method = order.Type == Order.BuyOrder ? "api/buy" : "api/sell";
            order.Price = Math.Round(order.Price, 2);
            var response = SendRequest(HttpVerbs.Post, method, order, true);

            if (response != null)
            {
                var myOrder = JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
                order = myOrder;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sells all available bitcoin in user account with market order
        /// </summary>
        /// <returns>True if order submitted successfully, false if order submission failed</returns>
        public bool SellAllMyBitcoin()
        {
            var accountBalance = GetAccountBalance();
            var order = new Order
            {
                IsMarketOrder = 1,
                Type = Order.SellOrder,
                Amount = accountBalance.BitcoinAvailable,
            };

            return SubmitOrder(ref order);
        }

        /// <summary>
        /// Buy bitcoin with all available MoneyBalance in user account with market order
        /// </summary>
        /// <returns>True if order submitted successfully, false if order submission failed</returns>
        public bool BuyWithAllMyMoney()
        {
            var accountBalance = GetAccountBalance();
            var order = new Order
            {
                IsMarketOrder = 1,
                Type = Order.BuyOrder,
                Total = accountBalance.MoneyAvailable,
            };

            return SubmitOrder(ref order);
        }

        /// <summary>
        /// Get the authenticated account's balance
        /// </summary>
        /// <returns>An object of type AccountBalance. Null if account balance cannot be retreived </returns>
        public AccountBalance GetAccountBalance()
        {
            AccountBalance result = null;
            var response = SendRequest(HttpVerbs.Get, "api/balance", false, true);
            if (response != null)
                result = JsonConvert.DeserializeObject<AccountBalance>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the authenticated account's latest transactions. Includes all balance changes. Buys, sells, deposits, withdrawals and fees.
        /// </summary>
        /// <returns>A list of object type UserTransOutput. Null if user tranasctions cannot be retreived </returns>
        public IList<UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending)
        {
            IList<UserTransOutput> result = null;

            var requestUri = "api/usertransactions?limit=" + limit + "&offset=" + offset;

            if (ascending)
                requestUri += "&sort=asc";
            else
                requestUri += "&sort=desc";

            var response = SendRequest(HttpVerbs.Get, requestUri, false, true);

            if (response != null)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<UserTransOutput[]>(content);
            }

            return result;
        }

        /// <summary>
        /// Cancels order with given OrderId
        /// </summary>
        /// <returns>True if order was cancelled, false otherwise</returns>
        public bool CancelOrder(Order order)
        {
            var result = false;

            var response = SendRequest(HttpVerbs.Post, "api/cancelOrder", new { id = order.Id }, true);
            if (response != null)
                result = true;

            return result;
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
            IList<Order> result = null;
            var response = SendRequest(HttpVerbs.Get, "api/openOrders", false, true);
            if (response != null)
                result = JsonConvert.DeserializeObject<IList<Order>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the market info ticker
        /// </summary>
        /// <returns>Returns a market ticker object if the request succeeded. Null otherwise</returns>
        public Ticker GetTicker()
        {
            Ticker result = null;
            var response = SendRequest(HttpVerbs.Get, "api/ticker", false, false);
            if (response != null)
                result = JsonConvert.DeserializeObject<Ticker>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the orderbook
        /// </summary>
        /// <returns>The orderbook. Null if there was an error</returns>
        public OrderBook GetOrderBook()
        {
            OrderBook result = null;
            var response = SendRequest(HttpVerbs.Get, "api/orderbook", false, false);
            if (response != null)
                result = JsonConvert.DeserializeObject<OrderBook>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the deposit money info
        /// </summary>
        /// <returns>The deposit money. Null if there was an error</returns>
        public DepositMoneyOutput GetDepositMoney()
        {
            DepositMoneyOutput result = null;

            var response = SendRequest(HttpVerbs.Get, "api/DepositMoney", false, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<DepositMoneyOutput>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Send the deposit money request, and return the deposit money request info.
        /// </summary>
        /// <returns>If a request is already, return the deposit money info. Null if there was an error</returns>
        public DepositMoneyOutput DepositMoney(DepositMoneyInput model)
        {
            DepositMoneyOutput result = null;

            var response = SendRequest(HttpVerbs.Post, "api/DepositMoney", model, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<DepositMoneyOutput>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the withdrawal money info
        /// </summary>
        /// <returns>The withdrawal money. Null if there was an error</returns>
        public WithdrawalMoneyOutput GetWithdrawalMoney()
        {
            WithdrawalMoneyOutput result = null;

            var response = SendRequest(HttpVerbs.Get, "api/WithdrawalMoney", false, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<WithdrawalMoneyOutput>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Send the withdrawal money request, and return the withdrawal money request info.
        /// </summary>
        /// <returns>If a request is already, return the withdrawal money info. Null if there was an error</returns>
        public WithdrawalMoneyOutput WithdrawalMoney(WithdrawalMoneyInput model)
        {
            WithdrawalMoneyOutput result = null;

            var response = SendRequest(HttpVerbs.Post, "api/WithdrawalMoney", model, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<WithdrawalMoneyOutput>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Cancel money requests Deposit with given RequestId
        /// </summary>
        /// <returns>True if request was cancelled, false otherwise</returns>
        public bool CancelDepositOperation(string balanceRequestId)
        {
            var result = false;

            var response = SendRequest(HttpVerbs.Delete, "api/DepositMoney/CancelOperation?balanceRequestId=" + balanceRequestId, false, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Cancel money requests Withdrawal with given RequestId
        /// </summary>
        /// <returns>True if request was cancelled, false otherwise</returns>
        public bool CancelWithdrawalOperation(string balanceRequestId)
        {
            var result = false;

            var response = SendRequest(HttpVerbs.Delete, "api/WithdrawalMoney/CancelOperation?balanceRequestId=" + balanceRequestId, false, true);

            if (response != null)
                result = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Get the last trades in the market
        /// </summary>
        /// <returns>Null if there was an error</returns>
        public IList<Trades> GetLastTrades(int last = 0)
        {
            IList<Trades> result = null;
            var url = "api/trades";
            if (last != 0)
                url = url + "?last=" + last;
            var response = SendRequest(HttpVerbs.Get, url, false, false);
            if (response != null)
                result = JsonConvert.DeserializeObject<IList<Trades>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        /// <summary>
        /// Handles the response received from the application. You can change this method to have custom error handling in your app.
        /// </summary>
        /// <returns>Returns false if there were no errors. True if request failed.</returns>
        private static bool RequestSucceeded(HttpResponseMessage response)
        {
            var result = true;
            if (!response.IsSuccessStatusCode)
            {
                //TODO: Write your own error handling code.
                Debug.WriteLine("Received error. Status code: " + response.StatusCode + ". Error message: " +
                                response.ReasonPhrase);
                result = false;
            }
            else
            {
                var json = response.Content.ReadAsAsync<dynamic>().Result;
                if (json is JObject && json["error"] != null)
                {
                    //TODO: Write your own error handling code.
                    Debug.WriteLine("Received error. Status code: " + (json["error"]["code"].ToString() as string) +
                                    ". Error message: " + (json["error"]["message"].ToString() as string));
                    result = false;
                }
            }

            return result;
        }

        private enum HttpVerbs
        {
            Get,
            Post,
            Delete
        }
    }
}