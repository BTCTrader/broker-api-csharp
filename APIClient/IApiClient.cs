﻿using System.Collections.Generic;
using BTCTrader.APIClient.Models;

namespace BTCTrader.APIClient
{
    public interface IApiClient
    {
        Ticker GetTicker();
        AccountBalance GetAccountBalance();
        IList<Order> GetOpenOrders();
        IList<UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending);
        bool CancelOrder(Order order);
    }
}