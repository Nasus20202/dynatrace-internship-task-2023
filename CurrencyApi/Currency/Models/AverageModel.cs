namespace CurrencyApi.Currency.Models;

public class AverageModel
{
    public string CurrencyCode { get; set; }
    public DateOnly Date { get; set; }
    public double ExchangeRate { get; set; }
    public AverageModel(string currencyCode, DateOnly date, double exchangeRate = 0) => (CurrencyCode, Date, ExchangeRate) = (currencyCode, date, exchangeRate);
}