namespace Marketplace.Framework;

public abstract class AggregateRoot<TId>
    where TId : RecordIdentifier
{
    public TId Id { get; set; }

    public AggregateRoot(TId id) => Id = id;
}