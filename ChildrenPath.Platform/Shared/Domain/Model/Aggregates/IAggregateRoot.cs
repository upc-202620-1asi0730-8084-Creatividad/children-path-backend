using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Shared.Domain.Model.Aggregates;

public interface IAggregateRoot
{
    /// <summary>
    ///     Gets the read-only collection of domain events recorded by this aggregate root.
    /// </summary>
    IReadOnlyCollection<IEvent> DomainEvents { get; }

    /// <summary>
    ///     Clears all recorded domain events from the aggregate root.
    /// </summary>
    void ClearDomainEvents();
}