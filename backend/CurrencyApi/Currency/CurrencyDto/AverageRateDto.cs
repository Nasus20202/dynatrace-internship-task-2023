namespace CurrencyApi.Currency.CurrencyDto;

public class AverageRateDto
{
    public string CurrencyCode { get; set; }
    public DateOnly Date { get; set; }
    public double ExchangeRate { get; set; }
    public AverageRateDto(string currencyCode, DateOnly date, double exchangeRate = 0) => (CurrencyCode, Date, ExchangeRate) = (currencyCode, date, exchangeRate);
}