using System.Collections.Generic;

namespace broker_api_csharp
{
    public interface IApiClient
    {
        ApiClient.Ticker GetTicker();
        ApiClient.AccountBalance GetAccountBalance();
        IList<ApiClient.Order> GetOpenOrders();
        IList<ApiClient.UserTransOutput> GetUserTransactions(int limit, int offset, bool ascending);
        bool CancelOrder(ApiClient.Order order);
    }
}