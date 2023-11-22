using Marketplace.Framework;

namespace Marketplace.Domain;

public record PictureId : RecordIdentifier
{
    public Guid Value { get; }

    public PictureId(Guid value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(value), "Picture id cannot be empty");

        Value = value;
    }

    public static implicit operator Guid(PictureId self) => self.Value;
}