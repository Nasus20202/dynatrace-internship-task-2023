namespace CurrencyApi.Currency.CurrencyService.DTO;

public class RateTableADto
{
    public class Rate
    {
        public string No { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public double Mid { get; set; }
        public Rate(string no, DateOnly effectiveDate, double mid) => (No, EffectiveDate, Mid) = (no, effectiveDate, mid);
        public DatedValue ToDatedValue() => new DatedValue(EffectiveDate, Mid);
    }
    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public Rate[] Rates { get; set; }
    public RateTableADto(string table, string currency, string code, Rate[] rates) => (Table, Currency, Code, Rates) = (table, currency, code, rates);
}