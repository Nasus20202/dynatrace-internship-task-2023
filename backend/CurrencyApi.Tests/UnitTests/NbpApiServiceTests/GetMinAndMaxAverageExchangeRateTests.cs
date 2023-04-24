using System.Net;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.CurrencyService.JsonFetcher;
using Microsoft.Extensions.Caching.Memory;

namespace Currency_API.Tests.UnitTests.NbpApiServiceTests;

public class GetMinAndMaxAverageExchangeRateTests
{
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_ValidInput()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/10/",
            "{\"table\":\"A\",\"currency\":\"dolaramerykański\",\"code\":\"USD\",\"rates\":["+
            "{\"no\":\"070/A/NBP/2023\",\"effectiveDate\":\"2023-04-11\",\"mid\":4.2917},"+
            "{\"no\":\"071/A/NBP/2023\",\"effectiveDate\":\"2023-04-12\",\"mid\":4.2713},"+
            "{\"no\":\"072/A/NBP/2023\",\"effectiveDate\":\"2023-04-13\",\"mid\":4.2225},"+
            "{\"no\":\"073/A/NBP/2023\",\"effectiveDate\":\"2023-04-14\",\"mid\":4.2042},"+
            "{\"no\":\"074/A/NBP/2023\",\"effectiveDate\":\"2023-04-17\",\"mid\":4.2261},"+
            "{\"no\":\"075/A/NBP/2023\",\"effectiveDate\":\"2023-04-18\",\"mid\":4.2151},"+
            "{\"no\":\"076/A/NBP/2023\",\"effectiveDate\":\"2023-04-19\",\"mid\":4.2244},"+
            "{\"no\":\"077/A/NBP/2023\",\"effectiveDate\":\"2023-04-20\",\"mid\":4.2024},"+
            "{\"no\":\"078/A/NBP/2023\",\"effectiveDate\":\"2023-04-21\",\"mid\":4.2006},"+
            "{\"no\":\"079/A/NBP/2023\",\"effectiveDate\":\"2023-04-24\",\"mid\":4.1905}]}");

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act
        var result = await service.GetMinAndMaxAverageExchangeRate("usd", 10);
        
        // Assert
        Assert.Equal(4.1905, result.min.Value);
        Assert.Equal(new DateOnly(2023, 4, 24), result.min.Date);
        Assert.Equal(4.2917, result.max.Value);
        Assert.Equal(new DateOnly(2023, 04, 11), result.max.Date);
    }
    
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_QuotationCountTooLow()
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
    public async Task GetMinAndMaxAverageExchangeRate_QuotationCountTooHigh()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/256/",
                string.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("usd", 256));
    }
    
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_InvalidCurrencyCode()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/invalid/last/10/",
                string.Empty, HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        
        // Act & Assert
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("invalid", 10));
    }
}