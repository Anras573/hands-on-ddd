namespace Marketplace.Domain;

public static class Events
{
    public class ClassifiedAdCreated
    {
        public Guid Id { get; init; }
        public Guid OwnerId { get; init; }
    }

    public class ClassifiedAdTitleChanged
    {
        public ClassifiedAdTitleChanged(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; init; }
        public string Title { get; init; }
    }

    public class ClassifiedAdTextUpdated
    {
        public ClassifiedAdTextUpdated(Guid id, string text)
        {
            Id = id;
            Text = text;
        }

        public Guid Id { get; init; }
        public string Text { get; init; }
    }

    public class ClassifiedAdPriceUpdated
    {
        public ClassifiedAdPriceUpdated(Guid id, decimal price, string currencyCode)
        {
            Id = id;
            Price = price;
            CurrencyCode = currencyCode;
        }

        public Guid Id { get; init; }
        public decimal Price { get; init; }
        public string CurrencyCode { get; init; }
    }

    public class ClassifiedAdSentForReview
    {
        public Guid Id { get; init; }
    }
}