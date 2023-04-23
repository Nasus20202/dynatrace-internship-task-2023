using CurrencyApi.Currency.CurrencyService.DTO;

namespace CurrencyApi.Currency.CurrencyService;

public interface IRatesService
{
    public Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date);
    public Task<(DatedValue min, DatedValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations);
    public Task<DatedValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations);
}