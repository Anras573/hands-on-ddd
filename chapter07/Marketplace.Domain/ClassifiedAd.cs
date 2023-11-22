using Marketplace.Domain.Exceptions;
using Marketplace.Framework;

namespace Marketplace.Domain;

public class ClassifiedAd : Entity<ClassifiedAdId>
{
    //public ClassifiedAdId Id { get; private set; }
    public UserId OwnerId { get; private set; }
    public ClassifiedAdTitle? Title { get; private set; }
    public ClassifiedAdText? Text { get; private set; }
    public Price? Price { get; private set; }
    public ClassifiedAdState State { get; private set; }
    public UserId? ApprovedBy { get; private set; }
    public List<Picture> Pictures { get; private set; } = new ();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ClassifiedAd(ClassifiedAdId id, UserId ownerId) : base(id)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        Apply(new Events.ClassifiedAdCreated(Id: id, OwnerId: ownerId));
    }

    public void SetTitle(ClassifiedAdTitle title)
    {
        Apply(new Events.ClassifiedAdTitleChanged(Id: Id, Title: title));
    }

    public void UpdateText(ClassifiedAdText text)
    {
        Apply(new Events.ClassifiedAdTextUpdated(Id: Id, Text: text));
    }

    public void UpdatePrice(Price price)
    {
        Apply(new Events.ClassifiedAdPriceUpdated(CurrencyCode: price.Currency.CurrencyCode ?? string.Empty, Id: Id, Price: price.Amount));
    }

    public void RequestToPublish()
    {
        Apply(new Events.ClassifiedAdSentForReview(Id: Id));
    }

    public void AddPicture(Uri pictureUri, PictureSize size)
    {
        Apply(new Events.PictureAddedToAClassifiedAd(
            PictureId: Guid.NewGuid(),
            ClassifiedAdId: Id,
            Url: pictureUri.ToString(),
            Height: size.Height,
            Width: size.Width
        ));
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case Events.ClassifiedAdCreated e:
                Id = new ClassifiedAdId(e.Id);
                OwnerId = new UserId(e.OwnerId);
                State = ClassifiedAdState.Inactive;
                break;
            case Events.ClassifiedAdTitleChanged e:
                Title = new ClassifiedAdTitle(e.Title);
                break;
            case Events.ClassifiedAdTextUpdated e:
                Text = new ClassifiedAdText(e.Text);
                break;
            case Events.ClassifiedAdPriceUpdated e:
                Price = new Price(e.Price, e.CurrencyCode);
                break;
            case Events.ClassifiedAdSentForReview:
                State = ClassifiedAdState.PendingReview;
                break;
            case Events.PictureAddedToAClassifiedAd e:
                Pictures.Add(new Picture(
                    new PictureId(e.PictureId),
                    new Uri(e.Url),
                    new PictureSize(e.Height, e.Width),
                    Pictures.Max(x => x.Order) + 1));
                break;
        }
    }

    protected override void EnsureValidState()
    {
        if (State == ClassifiedAdState.Inactive)
            return;

        var errors = new List<string>();

        if (Title is null)
            errors.Add($"{nameof(Title)} cannot be empty");

        if (Text is null)
            errors.Add($"{nameof(Text)} cannot be empty");

        if (Price is null || Price?.Amount == 0)
            errors.Add($"{nameof(Price)} cannot be zero");

        if (State == ClassifiedAdState.Active && ApprovedBy is null)
            errors.Add("Approver must be set");

        if (errors.Any())
            throw new InvalidEntityStateException(this, string.Join(", ", errors));
    }
}