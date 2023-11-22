using Marketplace.Framework;

namespace Marketplace.Domain;

public class Picture : Entity<PictureId>
{
    //public PictureId Id { get; set; }
    public PictureSize Size { get; private set; }
    public Uri Location { get; private set; }
    public int Order { get; private set; }

    public Picture(PictureId id, Uri location, PictureSize size, int order) : base(id)
    {
        Location = location;
        Size = size;
        Order = order;
    }
    protected override void When(object @event)
    {
        throw new NotImplementedException();
    }

    protected override void EnsureValidState()
    {
        // noop
    }
}