using ChildrenPath.Platform.Alerts.Domain.Model.Events;
using ChildrenPath.Platform.Shared.Application.Internal.EventHandlers;

namespace ChildrenPath.Platform.Alerts.Application.Internal.EventHandlers;

public class AlertCreatedEventHandler : IEventHandler<AlertCreatedEvent>
{
    public Task Handle(AlertCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }

    private static Task On(AlertCreatedEvent domainEvent)
    {
        Console.WriteLine("¡Nueva alerta creada! TripId: {0}, Tipo: {1}, Prioridad: {2}", 
            domainEvent.TripId, domainEvent.Type, domainEvent.Priority);
            
        return Task.CompletedTask;
    }
}