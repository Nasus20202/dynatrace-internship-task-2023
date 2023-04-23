namespace CurrencyApi.Currency.CurrencyService.Exceptions;
public class DataNotFoundException : Exception
{
    public DataNotFoundException(string message) : base(message)
    {
    }
}