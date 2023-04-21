using CurrencyApi.RatesApi;

namespace Currency_API.Tests;

public class NbpApiTest
{
    [Fact]
    public void UsdTest1()
    {
        var nbpApi = new NbpApi();
        var result = nbpApi.GetAverageExchangeRate("USD", new DateTime(2021, 1, 1)).Result;
        Assert.InRange(result, 2, 5);
    }
}