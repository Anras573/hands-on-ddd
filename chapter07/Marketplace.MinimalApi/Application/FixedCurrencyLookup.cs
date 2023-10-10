using Marketplace.Domain;

namespace Marketplace.MinimalApi.Application;

public class FixedCurrencyLookup : ICurrencyLookup
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
    };
    
    public CurrencyDetails FindCurrency(string currencyCode)
    {
        var currency = Currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode, CurrencyDetails.None);
        return currency;
    }
}