using System.Net;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.CurrencyService.JsonFetcher;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.UnitTests.NbpApiServiceTests;

public class GetMaxDifferenceBetweenBuyAndAskTests
{
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_ValidInput()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/c/usd/last/5/",
            "{\"table\":\"C\",\"currency\":\"dolaramerykański\",\"code\":\"USD\",\"rates\":"+
            "[{\"no\":\"075/C/NBP/2023\",\"effectiveDate\":\"2023-04-18\",\"bid\":4.1919,\"ask\":4.2765},"+
            "{\"no\":\"076/C/NBP/2023\",\"effectiveDate\":\"2023-04-19\",\"bid\":4.1769,\"ask\":4.2613},"+
            "{\"no\":\"077/C/NBP/2023\",\"effectiveDate\":\"2023-04-20\",\"bid\":4.1677,\"ask\":4.2519},"+
            "{\"no\":\"078/C/NBP/2023\",\"effectiveDate\":\"2023-04-21\",\"bid\":4.1532,\"ask\":4.2372},"+
            "{\"no\":\"079/C/NBP/2023\",\"effectiveDate\":\"2023-04-24\",\"bid\":4.1629,\"ask\":4.2471}]}");

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act
        var result = await service.GetMaxDifferenceBetweenBuyAndAsk("usd", 5);
        
        // Assert
        Assert.Equal(0.0846, result.Value);
        Assert.Equal(new DateOnly(2023, 4, 18), result.Date);
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_QuotationCountTooLow()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/0/",
            string.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("usd", 0));
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_QuotationCountTooHigh()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/256/",
                string.Empty, HttpStatusCode.BadRequest);
        
        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<FetchFailedException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("usd", 256));
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_InvalidCurrencyCode()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/invalid/last/10/",
            String.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("invalid", 10));
    }
}