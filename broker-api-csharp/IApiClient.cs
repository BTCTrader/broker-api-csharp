﻿using System.Collections.Generic;
using broker_api_csharp.Models;

namespace broker_api_csharp
{
    public interface IApiClient
    {
        ApiClient.Ticker GetTicker();
        ApiClient.AccountBalance GetAccountBalance();
        IList<Order> GetOpenOrders();
        IList<UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending);
        bool CancelOrder(Order order);
    }
}