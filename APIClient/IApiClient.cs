using System.Collections.Generic;
using BTCTrader.APIClient.Models;

namespace BTCTrader.APIClient
{
    public interface IApiClient
    {
        Ticker GetTicker();
        Ticker GetTicker(string pairSymbol);
        AccountBalance GetAccountBalance(string pairSymbol);
        IList<Order> GetOpenOrders(string pairSymbol);
        IList<UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending);
        IList<UserTransOutput> GetUserTransactions();
        bool CancelOrder(Order order);
        IList<Trades> GetLastTrades(string pairSymbol, int last = 0);
    }
}