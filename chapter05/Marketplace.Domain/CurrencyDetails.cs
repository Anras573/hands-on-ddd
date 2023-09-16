namespace Marketplace.Domain;

public record CurrencyDetails
{
    public string? CurrencyCode { get; init; }
    public bool InUse { get; init; }
    public int DecimalPlaces { get; init; }

    public static CurrencyDetails None => new () { InUse = false };
}