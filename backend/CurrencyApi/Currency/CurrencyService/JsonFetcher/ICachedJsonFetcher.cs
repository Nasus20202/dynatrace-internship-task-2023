namespace CurrencyApi.Currency.CurrencyService.JsonFetcher;

public interface ICachedJsonFetcher
{
    public Task<T> GetData<T>(string url);
    public void SetCacheTimeToLive(int timeToLive);
}