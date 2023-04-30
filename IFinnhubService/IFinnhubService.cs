namespace IFinnhubService
{
    public interface IFinnhubService
    {
        Dictionary<string, object>? GetCompany(string stockSymbol);
        Dictionary<string, object>? GetStockPrice(string stockSymbol);

    }
}