using Marketplace.Domain.Tests.Fakes;

namespace Marketplace.Domain.Tests;

public class PriceTest
{
    private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();

    [Fact]
    public void Price_cannot_be_instantiated_with_a_negative_amount()
    {
        Assert.Throws<ArgumentException>(() => Price.FromDecimal(-1, "EUR", CurrencyLookup));
    }
    
    [Fact]
    public void FromString_and_FromDecimal_should_be_equal()
    {
        var firstAmount = Price.FromDecimal(5, "EUR", CurrencyLookup);
        var secondAmount = Price.FromString("5", "EUR", CurrencyLookup);
        Assert.Equal(firstAmount, secondAmount);
    }
}