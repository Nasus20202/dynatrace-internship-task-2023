namespace CurrencyApi.RatesApi;

public class CurrencyInfo
{
    public string Currency { get; set; } = String.Empty;
    public string Code { get; set; } = String.Empty;
    public double Mid { get; set; }
    public double Bid { get; set; }
    public double Ask { get; set; }
    
}