using CurrencyApi.RatesApi;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.NbpApiTests;

public class AverageTests
{
    private readonly NbpService _nbpService = new(new MemoryCache(new MemoryCacheOptions()), new TestHttpFactory());
    [Fact]
    public void Test1()
    {
        var result = _nbpService.GetAverageExchangeRate("usd", new DateOnly(2021, 1, 5)).Result;
        Assert.InRange(result, 2, 5);
    }
    
    [Fact]
    public void Test2()
    {
        var result = _nbpService.GetAverageExchangeRate("jpy", new DateOnly(2015, 12, 31)).Result;
        Assert.InRange(result, 0.01, 0.3);
    }
}