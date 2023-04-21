using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CurrencyApi.RatesApi;

public class NbpApi : IRatesApi
{
    private const string NbpApiUrl = "http://api.nbp.pl/api";

    public async Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date)
    {
        string url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/{date:yyyy-MM-dd}/";
        var json = await FetchJson(url);
        return json.rates[0].mid;
    }

    public async Task<(DateAndValue min, DateAndValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        string url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/last/{quotations}/";
        var json = await FetchJson(url);
        DateAndValue max = new(), min = new();
        max.Value = 0; min.Value = Double.MaxValue;
        foreach (var rate in json["rates"])
        {
            double mid = Double.Parse(rate.mid.ToString());
            if (max.Value < mid) {
                max.Date = DateOnly.Parse(rate.effectiveDate.ToString());
                max.Value = mid;
            }
            if(min.Value > mid) {
                min.Date = DateOnly.Parse(rate.effectiveDate.ToString());
                min.Value = mid;
            }
        }
        return (min, max);
    }

    public async Task<DateAndValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations)
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
            throw new FetchFailedException("Invalid JSON response from NBP API");
        return json;
    }
}