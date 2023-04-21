using CurrencyApi.RatesApi;

namespace Currency_API.Tests.NbpApiTests;

public class DifferencesTests
{
    [Fact]
    public void Test1()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetMaxDifferenceBetweenBuyAndAsk("eur", 250).Result;
        var value = result.Value;
        Assert.InRange(value, 0.0001, 0.3);
    }
    
    [Fact]
    public void Test2()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetMaxDifferenceBetweenBuyAndAsk("usd", 1).Result;
        var value = result.Value;
        Assert.InRange(value, 0.001, 0.3);
    }
}