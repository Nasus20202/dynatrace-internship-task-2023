using CurrencyApi.RatesApi;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.NbpApiTests;

public class CacheTests
{
    [Fact]
    public void Test1()
    {
        MemoryCache memoryCache = new(new MemoryCacheOptions());
        var nbpApi = new NbpApi(memoryCache);
        DateOnly date1 = new(2019, 10, 1), date2 = date1.AddDays(1), date3 = date2.AddDays(1); string currency = "eur";
        nbpApi.GetAverageExchangeRate(currency, date1).Wait();
        Assert.Equal(1, memoryCache.Count);
        nbpApi.GetAverageExchangeRate(currency, date2).Wait();
        Assert.Equal(2, memoryCache.Count);
        nbpApi.GetAverageExchangeRate(currency, date2).Wait();
        Assert.Equal(2, memoryCache.Count);
        nbpApi.GetAverageExchangeRate(currency, date3).Wait();
        Assert.Equal(3, memoryCache.Count);
        nbpApi.GetAverageExchangeRate(currency, date1).Wait();
        Assert.Equal(3, memoryCache.Count);
        nbpApi.GetMinAndMaxAverageExchangeRate(currency, 50).Wait();
        Assert.Equal(4, memoryCache.Count);
        nbpApi.GetMinAndMaxAverageExchangeRate(currency, 50).Wait();
        Assert.Equal(4, memoryCache.Count);
        nbpApi.GetAverageExchangeRate(currency, date1).Wait();
        Assert.Equal(4, memoryCache.Count);
    }
}