using System.Net;
using System.Net.Http.Json;
using CurrencyApi.Currency.CurrencyDtos;

namespace Currency_API.Tests.IntegrationTests.CurrencyControllerTests;

public class GetMaxDifferenceBetweenBuyAndAskTests
{
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_ValidInput()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/c/USD/last/5/",
                "{\"table\":\"C\",\"currency\":\"dolaramerykański\",\"code\":\"USD\",\"rates\":"+
                "[{\"no\":\"075/C/NBP/2023\",\"effectiveDate\":\"2023-04-18\",\"bid\":4.1919,\"ask\":4.2765},"+
                "{\"no\":\"076/C/NBP/2023\",\"effectiveDate\":\"2023-04-19\",\"bid\":4.1769,\"ask\":4.2613},"+
                "{\"no\":\"077/C/NBP/2023\",\"effectiveDate\":\"2023-04-20\",\"bid\":4.1677,\"ask\":4.2519},"+
                "{\"no\":\"078/C/NBP/2023\",\"effectiveDate\":\"2023-04-21\",\"bid\":4.1532,\"ask\":4.2372},"+
                "{\"no\":\"079/C/NBP/2023\",\"effectiveDate\":\"2023-04-24\",\"bid\":4.1629,\"ask\":4.2471}]}");

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/maxBuyAskDifference/usd/5/");

        var result = await response.Content.ReadFromJsonAsync<RateDifferenceDto>();
        
        // Assert
        Assert.Equal(0.0846, result!.MaxDifference.Value);
        Assert.Equal(new DateOnly(2023, 4, 18), result.MaxDifference.Date);
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_QuotationCountTooLow()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/c/USD/last/0/",
                string.Empty, HttpStatusCode.NotFound);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/maxBuyAskDifference/usd/0");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_QuotationCountTooHigh()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/c/USD/last/256/",
                string.Empty, HttpStatusCode.BadRequest);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/maxBuyAskDifference/usd/256");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMaxDifferenceBetweenBuyAndAsk_InvalidCurrencyCode()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/c/A/last/10/",
                string.Empty, HttpStatusCode.NotFound);
        
        var client = factory.CreateClient();
        
        // Act & Assert
        var response = await client.GetAsync("exchange/maxBuyAskDifference/A/10");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}