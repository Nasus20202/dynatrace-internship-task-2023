namespace CurrencyApi.Currency.CurrencyService.DTO;

public class RateTableCDto
{
    public class Rate
    {
        public string No { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double GetDifference() => Math.Round(Ask - Bid, 6);
        public Rate(string no, DateOnly effectiveDate, double bid, double ask) => (No, EffectiveDate, Bid, Ask) = (no, effectiveDate, bid, ask);
        public DatedValue ToDatedValue() => new DatedValue(EffectiveDate, GetDifference());
    }
    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public Rate[] Rates { get; set; }
    public RateTableCDto(string table, string currency, string code, Rate[] rates) => (Table, Currency, Code, Rates) = (table, currency, code, rates);
}