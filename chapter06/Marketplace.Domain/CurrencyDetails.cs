﻿namespace Marketplace.Domain;

public record CurrencyDetails
{
    public string? CurrencyCode { get; set; }
    public bool InUse { get; set; }
    public int DecimalPlaces { get; set; }

    public static CurrencyDetails None => new () { InUse = false };
}