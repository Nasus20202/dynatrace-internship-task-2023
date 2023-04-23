using CurrencyApi.Currency.CurrencyService.DTO;

namespace CurrencyApi.Currency.Models;

public class DifferenceModel
{
    public string CurrencyCode { get; set; }
    public double Quotations { get; set; }
    public DatedValue MaxDifference { get; set; }
    public DifferenceModel(string currencyCode, double quotations, DatedValue maxDifference) 
        => (CurrencyCode, Quotations, MaxDifference) = (currencyCode, quotations, maxDifference);
}