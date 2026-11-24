using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Shared.Domain.Model.Aggregates;

public interface IAggregateRoot
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }
    void ClearDomainEvents();
}