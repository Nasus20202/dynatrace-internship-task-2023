using System.Net;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyApi.Currency.CurrencyService.JsonFetcher;

public class CachedJsonFetcher : ICachedJsonFetcher
{
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _clientFactory;
    private int _cacheTimeToLiveInSecs = 60;
    
    public CachedJsonFetcher(IMemoryCache cache, IHttpClientFactory clientFactory) => (_cache, _clientFactory) = (cache, clientFactory);
    
    
    public void SetCacheTimeToLive(int timeToLive)
    {
        _cacheTimeToLiveInSecs = timeToLive;
    }

    public async Task<T> GetData<T>(string url)
    {
        return await CheckCache<T>(url);
    }

    private async Task<T> CheckCache<T>(string url)
    {
        if (!_cache.TryGetValue(url, out T? json))
        {
            json = await FetchData<T>(url);
            _cache.Set(url, json, TimeSpan.FromSeconds(_cacheTimeToLiveInSecs));
        }
        return json!;
    }
    
    private async Task<T> FetchData<T>(string url) 
    {
        var client = _clientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new DataNotFoundException("Data not found");
            if (response.StatusCode != HttpStatusCode.OK)
                throw new FetchFailedException("Failed to fetch data");

            var json = await response.Content.ReadFromJsonAsync<T>();
            if (json == null)
                throw new FetchFailedException("Invalid JSON response");
            return json;
        } catch(TaskCanceledException)
        {
            throw new FetchFailedException("Request timed out");
        }
    }
}