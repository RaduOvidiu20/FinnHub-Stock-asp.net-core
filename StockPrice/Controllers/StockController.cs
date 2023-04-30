using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContract;
using StockPrice.Models;

namespace StockPrice.Controllers
{
    [Route("[controller]")]
    public class StockController : Controller
    {
        private readonly StockOptions _options;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public StockController(IOptions<StockOptions> options, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _options = options.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;

        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public IActionResult Index()
        {
            // reset stock symbol if does not exist
            if (string.IsNullOrEmpty(_options.DefaultStockSymbol))
            {
                _options.DefaultStockSymbol = "MSFT";
            }

            //get company from finnhub
            Dictionary<string, object>? companyProfile = _finnhubService.GetCompany(_options.DefaultStockSymbol);

            // get stock price
            Dictionary<string, object>? stockPrice = _finnhubService.GetStockPrice(_options.DefaultStockSymbol);

            // create model object
            StockModel stockModel = new StockModel() { StockSymbol = _options.DefaultStockSymbol };

            //load data from finnhub
            if (companyProfile != null && stockPrice != null)
            {
                stockModel = new StockModel()
                {
                    StockSymbol = Convert.ToString(companyProfile["ticker"]),
                    StockName = Convert.ToString(companyProfile["name"]),
                    StockPrice = Convert.ToDouble(stockPrice["c"].ToString())
                };
            }

            //send finhub data to view

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockModel);
        }
    }
}
