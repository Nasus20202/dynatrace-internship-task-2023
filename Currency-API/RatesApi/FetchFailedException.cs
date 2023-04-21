namespace CurrencyApi.RatesApi;

public class FetchFailedException : Exception
{
    public FetchFailedException(string message) : base(message)
    {
    }
}