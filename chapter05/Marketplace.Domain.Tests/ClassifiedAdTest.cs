using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Tests.Fakes;

namespace Marketplace.Domain.Tests;

public class ClassifiedAdTest
{
    [Fact]
    public void Can_publish_a_valid_app()
    {
        var ad = GetClassifiedAd();

        ad.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        ad.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        ad.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

        ad.RequestToPublish();

        Assert.Equal(ClassifiedAdState.PendingReview, ad.State);
    }

    [Fact]
    public void Cannot_publish_without_title()
    {
        var ad = GetClassifiedAd();

        ad.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        ad.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_without_text()
    {
        var ad = GetClassifiedAd();

        ad.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        ad.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_without_price()
    {
        var ad = GetClassifiedAd();

        ad.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        ad.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));

        Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_with_zero_price()
    {
        var ad = GetClassifiedAd();

        ad.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
        ad.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
        ad.UpdatePrice(Price.FromDecimal(0, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
    }

    private static ClassifiedAd GetClassifiedAd() 
        => new(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
}