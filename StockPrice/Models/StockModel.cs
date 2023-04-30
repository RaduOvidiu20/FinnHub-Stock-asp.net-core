namespace StockPrice.Models
{
    public class StockModel
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double StockPrice { get; set; } = 0;
        public uint? Quantity { get; set; } = 0;

    }
}

