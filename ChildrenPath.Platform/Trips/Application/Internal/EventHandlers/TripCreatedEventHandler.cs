using ChildrenPath.Platform.Shared.Application.Internal.EventHandlers;
using ChildrenPath.Platform.Trips.Domain.Model.Events;

namespace ChildrenPath.Platform.Trips.Application.Internal.EventHandlers;

public class TripCreatedEventHandler : IEventHandler<TripCreatedEvent>
{
    public Task Handle(TripCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(TripCreatedEvent domainEvent)
    {
        Console.WriteLine("New trip created! TripId: {0}, Code: {1}, Route: {2}, Shift: {3}",
            domainEvent.TripId, domainEvent.Code, domainEvent.RouteName, domainEvent.Shift);
        return Task.CompletedTask;
    }
}
