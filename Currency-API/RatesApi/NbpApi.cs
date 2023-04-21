using System.Net;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace CurrencyApi.RatesApi;

public class NbpApi : IRatesApi
{
    private const string NbpApiUrl = "http://api.nbp.pl/api";

    public async Task<double> GetAverageExchangeRate(string currencyCode, DateTime date)
    {
        string url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/{date:yyyy-MM-dd}/";
        var json = await FetchJson(url);
        return 0;
    }

    public async Task<double> GetMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        throw new NotImplementedException();
    }

    public async Task<double> GetMinAverageExchangeRate(string currencyCode, int quotations)
    {
        throw new NotImplementedException();
    }

    public async Task<double> GetMaxDifferenceBetweenBuyAndSell(string currencyCode, int quotations)
    {
        throw new NotImplementedException();
    }

    private async Task<dynamic> FetchJson(string url)
    {
        HttpResponseMessage? response;
        using (var client = new HttpClient())
        {
            response = await client.GetAsync(url);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new DataNotFoundException("Data not found in NBP API");
        if (response.StatusCode != HttpStatusCode.OK || responseContent == null)
            throw new FetchFailedException("Failed to fetch data from NBP API");

        dynamic? json = JsonConvert.DeserializeObject(responseContent);
        if (json == null)
            throw new JsonSerializationException("Invalid JSON response from NBP API");
        return json;
    }
}