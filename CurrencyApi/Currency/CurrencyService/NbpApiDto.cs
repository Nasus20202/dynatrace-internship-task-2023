namespace CurrencyApi.Currency.CurrencyService;

public class NbpApiDto<T> where T : INbpApiRate {
    public NbpApiDto(string table, string currency, string code, T[] rates)
    {
        Table = table;
        Currency = currency;
        Code = code;
        Rates = rates;
    }

    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public T[] Rates { get; set; }
}

public interface INbpApiRate {
    public double GetRate();
}

public abstract class NbpRate : INbpApiRate
{
    public abstract double GetRate();
    public string No { get; set; } = String.Empty;
    public DateOnly EffectiveDate { get; set; }
}

public class NbpApiRateTableA : NbpRate {
    public double Mid { get; set; }
    public override double GetRate() => Mid;
}

public class NbpApiRateTableC : NbpRate
{
    public double Bid { get; set; }
    public double Ask { get; set; }

    public override double GetRate() => (Bid + Ask) / 2;
    public double GetDifference() => Math.Round(Ask - Bid, 6); // round to remove floating point errors
}