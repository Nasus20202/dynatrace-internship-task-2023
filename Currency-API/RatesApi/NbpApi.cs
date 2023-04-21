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
        var json = await FetchJson<NbpApiRateTableA>(url);
        return json.Rates[0].GetRate();
    }

    public async Task<(DateAndValue min, DateAndValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        string url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/last/{quotations}/";
        var json = await FetchJson<NbpApiRateTableA>(url);
        DateAndValue max = new(), min = new();
        max.Value = 0; min.Value = Double.MaxValue;
        foreach (var rate in json.Rates)
        {
            if (max.Value < rate.Mid)
            {
                max.Date = rate.EffectiveDate;
                max.Value = rate.Mid;
            }
            if(min.Value > rate.Mid)
            {
                min.Date = rate.EffectiveDate;
                min.Value = rate.Mid;
            }
        }
        return (min, max);
    }

    public async Task<DateAndValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations)
    {
        throw new NotImplementedException();
    }

    
    private async Task<NbpApiDto<T>> FetchJson<T>(string url) where T : INbpApiRate
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

        NbpApiDto<T>? json = JsonConvert.DeserializeObject<NbpApiDto<T>>(responseContent);
        if (json == null)
            throw new FetchFailedException("Invalid JSON response from NBP API");
        return json;
    }
}