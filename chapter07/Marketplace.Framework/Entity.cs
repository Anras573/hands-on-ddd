﻿namespace Marketplace.Framework;

public abstract class Entity<TId>
    where TId : RecordIdentifier
{

    public TId Id { get; set; }
    private readonly List<object> _events = new();
    
    protected Entity(TId id) => Id = id;

    protected void Apply(object @event)
    {
        When(@event);
        EnsureValidState();
        _events.Add(@event);
    }

    protected abstract void When(object @event);

    public IEnumerable<object> GetChanges() => _events.AsEnumerable();

    public void ClearChanges() => _events.Clear();

    protected abstract void EnsureValidState();
}