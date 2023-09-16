namespace Marketplace.Domain;

public record Price : Money
{
    public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
        => new(amount, currency, currencyLookup);
    public new static Price FromString(string amount, string currency, ICurrencyLookup currencyLookup)
        => new(decimal.Parse(amount), currency, currencyLookup);

    private Price(decimal amount, string currency, ICurrencyLookup currencyLookup) : base(amount, currency, currencyLookup)
    {
        if (amount < 0)
            throw new ArgumentException("Price cannot be negative", nameof(amount));
    }

    internal Price(decimal amount, string currency) : base(amount, new CurrencyDetails { CurrencyCode = currency }) { }
    
    public Price(decimal amount, CurrencyDetails currency) : base(amount, currency) { }
}