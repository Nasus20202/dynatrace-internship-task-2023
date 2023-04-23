namespace CurrencyApi.Currency.CurrencyService.Models;

public class RateTableCModel
{
    public RateTableCModel(string table, string currency, string code, Rate[] rates) => (Table, Currency, Code, Rates) = (table, currency, code, rates);
    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public Rate[] Rates { get; set; }

    public class Rate
    {
        public Rate(string no, DateOnly effectiveDate, double bid, double ask) => (No, EffectiveDate, Bid, Ask) = (no, effectiveDate, bid, ask);
        public string No { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double GetDifference() => Math.Round(Ask - Bid, 6);
        public DatedValue ToDatedValue() => new DatedValue(EffectiveDate, GetDifference());
    }
}