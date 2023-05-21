namespace Marketplace.Domain;

public record ClassifiedAdText(string Value)
{
    public static ClassifiedAdText FromString(string value) => new (value);

    public static implicit operator string(ClassifiedAdText self) => self.Value;
}