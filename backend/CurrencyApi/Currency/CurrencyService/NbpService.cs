using CurrencyApi.Currency.CurrencyService.DTO;
using CurrencyApi.Currency.CurrencyService.JsonFetcher;

namespace CurrencyApi.Currency.CurrencyService;

public class NbpService : IRatesService
{
    private const string NbpApiUrl = "https://api.nbp.pl/api";
    private readonly ICachedJsonFetcher _jsonFetcher;
    private const int CacheTimeToLive = 15*60; // 15 minutes

    public NbpService(ICachedJsonFetcher jsonFetcher)
    { 
        _jsonFetcher = jsonFetcher;
        _jsonFetcher.SetCacheTimeToLive(CacheTimeToLive);
    }

    public async Task<double> GetAverageExchangeRate(string currencyCode, DateOnly date)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/{date:yyyy-MM-dd}/";
        var json = await _jsonFetcher.GetData<RateTableADto>(url);
        return json.Rates.First().Mid;
    }
    
    public async Task<(DatedValue min, DatedValue max)> GetMinAndMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/a/{currencyCode}/last/{quotations}/";
        var json = await _jsonFetcher.GetData<RateTableADto>(url);
        var sorted = json.Rates.OrderBy(rate => rate.Mid).ToArray();
        return (sorted.First().ToDatedValue(), sorted.Last().ToDatedValue());
    }

    public async Task<DatedValue> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations)
    {
        var url = $"{NbpApiUrl}/exchangerates/rates/c/{currencyCode}/last/{quotations}/";
        var json = await _jsonFetcher.GetData<RateTableCDto>(url);
        var sorted = json.Rates.OrderBy(rate => rate.GetDifference());
        return sorted.Last().ToDatedValue();
    }
}