namespace CurrencyApi.RatesApi.Exceptions;

public class FetchFailedException : Exception
{
    public FetchFailedException(string message) : base(message)
    {
    }
}