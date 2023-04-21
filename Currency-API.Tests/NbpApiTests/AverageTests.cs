using CurrencyApi.RatesApi;

namespace Currency_API.Tests.NbpApiTests;

public class AverageTests
{
    [Fact]
    public void Test1()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetAverageExchangeRate("usd", new DateOnly(2021, 1, 5)).Result;
        Assert.InRange(result, 2, 5);
    }
    
    [Fact]
    public void Test2()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetAverageExchangeRate("jpy", new DateOnly(2015, 12, 31)).Result;
        Assert.InRange(result, 0.01, 0.3);
    }
}