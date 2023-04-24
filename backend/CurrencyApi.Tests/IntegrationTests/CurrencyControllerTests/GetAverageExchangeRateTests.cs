using System.Net;
using System.Net.Http.Json;
using CurrencyApi.Currency.CurrencyDtos;

namespace Currency_API.Tests.IntegrationTests.CurrencyControllerTests;

public class GetAverageExchangeRateTests
{
    [Fact]
    public async Task GetAverageExchangeRate_ValidInput()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/KRW/2022-09-16/",
                "{\"table\":\"A\",\"currency\":\"wonpołudniowokoreański\",\"code\":\"KRW\",\"rates\":[{\"no\":\"180/A/NBP/2022\",\"effectiveDate\":\"2022-09-16\",\"mid\":0.003398}]}");

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/average/krw/2022-09-16/");
        var s = await response.Content.ReadAsStringAsync();        
        var result = await response.Content.ReadFromJsonAsync<AverageRateDto>();
        
        // Assert
        Assert.Equal(0.003398, result!.ExchangeRate);
        Assert.Equal(new DateOnly(2022, 9, 16), result.Date);
    }
    
    [Fact]
    public async Task GetAverageExchangeRate_InvalidDate()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/KRW/2023-01-10/",
                string.Empty, HttpStatusCode.NotFound);

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/average/krw/2023-01-10/");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAverageExchangeRate_NonExistentCode()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a/ABC/2022-09-16/",
                string.Empty, HttpStatusCode.NotFound);

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/average/ABC/2022-09-16/");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAverageExchangeRate_InvalidCodeFormat()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>()
            .AddResponse("https://api.nbp.pl/api/exchangerates/rates/a//2022-09-16/",
                string.Empty, HttpStatusCode.NotFound);

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync($"exchange/average//2022-09-16/");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}