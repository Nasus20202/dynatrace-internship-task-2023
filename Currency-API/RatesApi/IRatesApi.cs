namespace CurrencyApi.RatesApi;

public interface IRatesApi
{
    public Task<double> GetAverageExchangeRate(string currencyCode, DateTime date);
    public Task<double> GetMaxAverageExchangeRate(string currencyCode, int quotations);
    public Task<double> GetMinAverageExchangeRate(string currencyCode, int quotations);
    public Task<double> GetMaxDifferenceBetweenBuyAndSell(string currencyCode, int quotations);
}