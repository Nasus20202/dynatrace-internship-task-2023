using System.Net;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.CurrencyService.JsonFetcher;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.UnitTests.NbpApiServiceTests;

public class GetAverageExchangeRateTests
{
    [Fact]
    public async Task GetAverageExchangeRate_ValidInput()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/2023-04-12/",
                "{\"table\":\"A\",\"currency\":\"dolar amerykański\",\"code\":\"USD\"," + 
                "\"rates\":[{\"no\":\"071/A/NBP/2023\",\"effectiveDate\":\"2023-04-12\",\"mid\":4.2713}]}");

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act
        var result = await service.GetAverageExchangeRate("usd", new DateOnly(2023, 4, 12));
        
        // Assert
        Assert.Equal(4.2713, result);
    }

    [Fact]
    public async Task GetAverageExchangeRate_InvalidDate()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/2023-01-10/",
                string.Empty, HttpStatusCode.NotFound);
        
        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetAverageExchangeRate("usd", new DateOnly(2023, 1, 10)));
    }

    [Fact]
    public async Task GetAverageExchangeRate_NonExistentCode()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/aaa/2023-04-12/",
            string.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetAverageExchangeRate("aaa", new DateOnly(2023, 4, 12)));
    }
    
    [Fact]
    public async Task GetAverageExchangeRate_InvalidCodeFormat()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/us/2023-04-12/",
                string.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetAverageExchangeRate("us", new DateOnly(2023, 4, 12)));
    }
}