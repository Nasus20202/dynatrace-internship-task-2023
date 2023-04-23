namespace CurrencyApi.Currency.CurrencyService.Models;

public class DatedValue
{
    public DateOnly Date { get; set; }
    public double Value { get; set; }
    public DatedValue(DateOnly date, double value = 0) => (Date, Value) = (date, value);
}