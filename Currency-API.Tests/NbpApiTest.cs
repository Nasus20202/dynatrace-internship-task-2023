using CurrencyApi.RatesApi;

namespace Currency_API.Tests;

public class NbpApiTest
{
    [Fact]
    public void UsdTest1()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetAverageExchangeRate("usd", new DateOnly(2021, 1, 5)).Result;
        Assert.InRange(result, 2, 5);
    }
    
    [Fact]
    public void KrwTest1()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetMinAndMaxAverageExchangeRate("krw", 50).Result;
        var min = result.min;
        var max = result.max;
        Assert.InRange(min, 0.0001, 0.03);
        Assert.InRange(max, 0.0001, 0.03);
    }
}