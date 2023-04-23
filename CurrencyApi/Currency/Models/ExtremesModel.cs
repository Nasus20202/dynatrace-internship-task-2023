using CurrencyApi.Currency.CurrencyService.DTO;

namespace CurrencyApi.Currency.Models;

public class ExtremesModel
{
    public string CurrencyCode { get; set; }
    public int Quotations { get; set; }
    public DatedValue MinExchangeRate { get; set; }
    public DatedValue MaxExchangeRate { get; set; }
    public ExtremesModel(string currencyCode, int quotations, DatedValue minExchangeRate, DatedValue maxExchangeRate) 
        => (CurrencyCode, Quotations, MinExchangeRate, MaxExchangeRate) = (currencyCode, quotations, minExchangeRate, maxExchangeRate);
}