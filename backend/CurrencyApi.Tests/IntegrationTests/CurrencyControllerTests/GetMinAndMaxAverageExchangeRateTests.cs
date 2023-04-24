using System.Net;
using System.Net.Http.Json;
using CurrencyApi.Currency.CurrencyDtos;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Currency_API.Tests.IntegrationTests.CurrencyControllerTests;

public class GetMinAndMaxAverageExchangeRateTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_ValidInput()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/EUR/last/5/",
                "{\"table\":\"A\",\"currency\":\"euro\",\"code\":\"EUR\",\"rates\":"+
                "[{\"no\":\"075/A/NBP/2023\",\"effectiveDate\":\"2023-04-18\",\"mid\":4.6286},"+
                "{\"no\":\"076/A/NBP/2023\",\"effectiveDate\":\"2023-04-19\",\"mid\":4.6278},"+
                "{\"no\":\"077/A/NBP/2023\",\"effectiveDate\":\"2023-04-20\",\"mid\":4.6109},"+
                "{\"no\":\"078/A/NBP/2023\",\"effectiveDate\":\"2023-04-21\",\"mid\":4.6039},"+
                "{\"no\":\"079/A/NBP/2023\",\"effectiveDate\":\"2023-04-24\",\"mid\":4.6129}]}");

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/extremes/eur/5/");

        var result = await response.Content.ReadFromJsonAsync<ExtremeRatesDto>();
        
        // Assert
        Assert.Equal(4.6039, result!.MinExchangeRate.Value);
        Assert.Equal(new DateOnly(2023, 4, 21), result.MinExchangeRate.Date);
        Assert.Equal(4.6286, result.MaxExchangeRate.Value);
        Assert.Equal(new DateOnly(2023, 04, 18), result.MaxExchangeRate.Date);
    }
    
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_QuotationCountTooLow()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/USD/last/0/",
                string.Empty, HttpStatusCode.NotFound);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/extremes/usd/0");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_QuotationCountTooHigh()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/USD/last/256/",
                string.Empty, HttpStatusCode.BadRequest);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/extremes/usd/256");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMinAndMaxAverageExchangeRate_InvalidCurrencyCode()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/A/last/10/",
                string.Empty, HttpStatusCode.NotFound);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/extremes/A/10");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}