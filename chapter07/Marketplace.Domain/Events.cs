namespace Marketplace.Domain;

public static class Events
{
    public record ClassifiedAdCreated(Guid Id, Guid OwnerId);

    public record ClassifiedAdTitleChanged(Guid Id, string Title);

    public record ClassifiedAdTextUpdated(Guid Id, string Text);
    
    public record ClassifiedAdPriceUpdated(Guid Id, decimal Price, string CurrencyCode);

    public record ClassifiedAdSentForReview(Guid Id);
    
    public record PictureAddedToAClassifiedAd(Guid PictureId, Guid ClassifiedAdId, string Url, int Height, int Width);
}