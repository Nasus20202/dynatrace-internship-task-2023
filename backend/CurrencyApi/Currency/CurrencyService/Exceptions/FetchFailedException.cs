namespace CurrencyApi.Currency.CurrencyService.Exceptions;

public class FetchFailedException : Exception
{
    public FetchFailedException(string message) : base(message)
    {
    }
}