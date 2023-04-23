using CurrencyApi.RatesApi;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.NbpApiTests;

public class ExtremesTests
{
    private readonly NbpService _nbpService = new(new MemoryCache(new MemoryCacheOptions()), new TestHttpFactory());
    [Fact]
    public void Test1()
    {
        var result = _nbpService.GetMinAndMaxAverageExchangeRate("krw", 50).Result;
        var min = result.min.Value;
        var max = result.max.Value;
        Assert.InRange(min, 0.0001, 0.03);
        Assert.InRange(max, 0.0001, 0.03);
        Assert.True(min <= max);
    }
    
    [Fact]
    public void Test2()
    {
        var result = _nbpService.GetMinAndMaxAverageExchangeRate("CZK", 250).Result;
        var min = result.min.Value;
        var max = result.max.Value;
        Assert.InRange(min, 0.005, 0.5);
        Assert.InRange(min, 0.005, 0.5);
        Assert.True(min <= max);
    }
}