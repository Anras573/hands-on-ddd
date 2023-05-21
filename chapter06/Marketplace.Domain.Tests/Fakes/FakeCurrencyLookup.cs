﻿namespace Marketplace.Domain.Tests.Fakes;

public class FakeCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<CurrencyDetails> _currencies = new[]
    {
        new CurrencyDetails
        {
            CurrencyCode = "EUR",
            DecimalPlaces = 2,
            InUse = true
        },
        new CurrencyDetails
        {
            CurrencyCode = "USD",
            DecimalPlaces = 2,
            InUse = true
        },
        new CurrencyDetails
        {
            CurrencyCode = "JPY",
            DecimalPlaces = 0,
            InUse = true
        },
        new CurrencyDetails
        {
            CurrencyCode = "DEM",
            DecimalPlaces = 2,
            InUse = false
        }
    };

    public CurrencyDetails FindCurrency(string currencyCode)
        => _currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode, CurrencyDetails.None);
}