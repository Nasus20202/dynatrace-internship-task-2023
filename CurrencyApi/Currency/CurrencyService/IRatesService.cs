namespace CurrencyApi.Currency.CurrencyService;

public class DateAndValue
{
    public DateOnly Date { get; set; }
    public double Value { get; set; }
    public DateAndValue(double value = 0) => Value = value;
}

public interface IRatesService
{
    public Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date);
    public Task<(DateAndValue min, DateAndValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations);
    public Task<DateAndValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations);
}