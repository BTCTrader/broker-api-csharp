namespace BTCTrader.APIClient.Models
{
    public class UserTransInput
    {
        public string Sort { get; set; } // sorting by desc, asc
        public int Offset { get; set; } // number of skiping records
        public int Limit { get; set; } // number of limit query
    }
}
