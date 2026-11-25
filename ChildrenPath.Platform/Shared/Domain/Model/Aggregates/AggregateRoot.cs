using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Shared.Domain.Model.Aggregates;

public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<IEvent> _domainEvents = [];

    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    // 👇 ESTE ES EL MÉTODO QUE TE FALTA
    protected void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}