namespace CurrencyApi.Currency.Exceptions;

public class InvalidQuotationsCountException : Exception
{
    public InvalidQuotationsCountException(string message) : base(message) { }
}