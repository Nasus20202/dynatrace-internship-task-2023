﻿using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.CurrencyService.DTO;

namespace CurrencyApi.Currency.CurrencyService;

public class NbpService : IRatesService
{
    private const string NbpApiUrl = "https://api.nbp.pl/api";
    private const int CacheTimeToLiveInSecs = 15*60;
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _clientFactory;

    public NbpService(IMemoryCache cache, IHttpClientFactory clientFactory) => (_cache, _clientFactory) = (cache, clientFactory);

    // Returns average exchange rate for given currency code and date
    public async Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/{date:yyyy-MM-dd}/";
        var json = await GetData<NbpApiRateTableA>(url);
        return json.Rates[0].GetRate();
    }

    // Returns max average exchange rate for given currency code and number of quotations
    public async Task<(DatedValue min, DatedValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/last/{quotations}/";
        var json = await GetData<NbpApiRateTableA>(url);
        var sorted = json.Rates.OrderBy(rate => rate.Mid).ToList();
        var minRate = sorted.First();
        var maxRate = sorted.Last();
        return (new DatedValue(minRate.EffectiveDate, minRate.Mid), new DatedValue(maxRate.EffectiveDate, maxRate.Mid));
    }

    // Returns max difference between buy and sell price with dates for given currency code and number of quotations
    public async Task<DatedValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations)
    {
        string url = $"{NbpApiUrl}/exchangerates/rates/c/{currencyCode}/last/{quotations}/";
        var json = await GetData<NbpApiRateTableC>(url);
        var sorted = json.Rates.OrderBy(rate => rate.GetDifference());
        var max = sorted.Last();
        return new DatedValue(max.EffectiveDate, max.GetDifference());
    }

    // Returns JSON from NBP API with requested table type
    private async Task<NbpApiDto<T>> GetData<T>(string url) where T : INbpApiRate
    {
        return await CheckCache(url, async () => await FetchData<T>(url));
    }
    
    private async Task<NbpApiDto<T>> CheckCache<T>(string key, Func<Task<NbpApiDto<T>>> fetch) where T : INbpApiRate
    {
        if (!_cache.TryGetValue(key, out NbpApiDto<T>? json))
        {
            json = await fetch();
            _cache.Set(key, json, TimeSpan.FromSeconds(CacheTimeToLiveInSecs));
        }
        return json!;
    }
    
    private async Task<NbpApiDto<T>> FetchData<T>(string url) where T : INbpApiRate
    {
        HttpResponseMessage? response;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            response = await client.SendAsync(request);
        } catch(TaskCanceledException)
        {
            throw new FetchFailedException("NBP API request timed out");
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