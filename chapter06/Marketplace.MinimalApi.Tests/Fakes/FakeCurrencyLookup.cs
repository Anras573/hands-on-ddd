using Marketplace.Domain;

namespace Marketplace.MinimalApi.Tests.Fakes;

public class FakeCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<CurrencyDetails> Currencies = new[]
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
        => Currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode, CurrencyDetails.None);
}