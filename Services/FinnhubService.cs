using Microsoft.Extensions.Configuration;
using ServiceContract;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Dictionary<string, object>? GetCompany(string stockSymbol)
        {
            //http client

            HttpClient httpClient = _httpClientFactory.CreateClient();

            //http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //send request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            //response body
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //convert response to dictionary
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            //checking exceptions
            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from server");
            }
            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }
            //returning response dictionary
            return responseDictionary;
        }

        public Dictionary<string, object>? GetStockPrice(string stockSymbol)
        {
            //http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
            };

            //send request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            // read response
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //convert response to dictionary
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            //checking exceptions
            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from server");
            }
            else if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }

            //return response dictionary to the caller
            return responseDictionary;
        }
    }
}
