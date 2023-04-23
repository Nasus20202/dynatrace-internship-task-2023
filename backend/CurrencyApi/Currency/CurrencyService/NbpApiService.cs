using CurrencyApi.Currency.CurrencyService.JsonFetcher;
using CurrencyApi.Currency.CurrencyService.Models;

namespace CurrencyApi.Currency.CurrencyService;

public class NbpApiService : IRatesService
{
    private const string NbpApiUrl = "https://api.nbp.pl/api";
    private readonly ICachedJsonFetcher _jsonFetcher;
    private const int CacheTimeToLive = 15*60; // 15 minutes

    public NbpApiService(ICachedJsonFetcher jsonFetcher)
    { 
        _jsonFetcher = jsonFetcher;
        _jsonFetcher.SetCacheTimeToLive(CacheTimeToLive);
    }

    public async Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/{date:yyyy-MM-dd}/";
        var json = await _jsonFetcher.GetData<RateTableAModel>(url);
        return json.Rates.First().Mid;
    }
    
    public async Task<(DatedValue min, DatedValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/last/{quotations}/";
        var json = await _jsonFetcher.GetData<RateTableAModel>(url);
        var sorted = json.Rates.OrderBy(rate => rate.Mid).ToArray();
        return (sorted.First().ToDatedValue(), sorted.Last().ToDatedValue());
    }

    public async Task<DatedValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/c/{currencyCode}/last/{quotations}/";
        var json = await _jsonFetcher.GetData<RateTableCModel>(url);
        var sorted = json.Rates.OrderBy(rate => rate.GetDifference());
        return sorted.Last().ToDatedValue();
    }
}