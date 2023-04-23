namespace CurrencyApi.Currency.ErrorHandling;

public class ErrorContext
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public ErrorContext(string message, int statusCode) => (Message, StatusCode) = (message, statusCode);
}