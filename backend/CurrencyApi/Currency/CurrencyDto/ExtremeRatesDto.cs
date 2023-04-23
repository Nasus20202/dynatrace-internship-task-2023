using CurrencyApi.Currency.CurrencyService.DTO;

namespace CurrencyApi.Currency.CurrencyDto;


public class ExtremeRatesDto
{
    public string CurrencyCode { get; set; }
    public int Quotations { get; set; }
    public DatedValue MinExchangeRate { get; set; }
    public DatedValue MaxExchangeRate { get; set; }
    public ExtremeRatesDto(string currencyCode, int quotations, DatedValue minExchangeRate, DatedValue maxExchangeRate) 
        => (CurrencyCode, Quotations, MinExchangeRate, MaxExchangeRate) = (currencyCode, quotations, minExchangeRate, maxExchangeRate);
}