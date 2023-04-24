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
        var result = await service.GetMaxDifferenceBetweenBuyAndAsk("usd", 5);
        Assert.Equal(0.0846, result.Value);
        Assert.Equal(new DateOnly(2023, 4, 18), result.Date);
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_InvalidQuotationCount()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/0/",
                "﻿404 NotFound", HttpStatusCode.NotFound)
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/usd/last/256/",
                "400 BadRequest - Przekroczony limit 255 wyników / Maximum size of 255 data series has been exceeded", HttpStatusCode.BadRequest);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("usd", 0));
        await Assert.ThrowsAsync<FetchFailedException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("usd", 256));
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_InvalidCurrencyCode()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var httpClientFactory = new MockHttpClientFactory();

        httpClientFactory.AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/invalid/last/10/",
            "﻿404 NotFound", HttpStatusCode.NotFound);

        var service = new NbpApiService(new CachedJsonFetcher(cache, httpClientFactory));
        await Assert.ThrowsAsync<DataNotFoundException>(async () =>
            await service.GetMinAndMaxAverageExchangeRate("invalid", 10));
    }
}