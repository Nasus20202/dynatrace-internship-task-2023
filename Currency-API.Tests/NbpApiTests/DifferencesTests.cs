using CurrencyApi.RatesApi;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.NbpApiTests;

public class DifferencesTests
{
    private readonly NbpApi _nbpApi = new(new MemoryCache(new MemoryCacheOptions()));
    [Fact]
    public void Test1()
    {
        var result = _nbpApi.GetMaxDifferenceBetweenBuyAndAsk("eur", 250).Result;
        var value = result.Value;
        Assert.InRange(value, 0.0001, 0.3);
    }
    
    [Fact]
    public void Test2()
    {
        var result = _nbpApi.GetMaxDifferenceBetweenBuyAndAsk("usd", 1).Result;
        var value = result.Value;
        Assert.InRange(value, 0.001, 0.3);
    }
}