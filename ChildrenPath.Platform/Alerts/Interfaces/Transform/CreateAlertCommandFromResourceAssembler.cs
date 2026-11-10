using ChildrenPath.Platform.Alerts.Domain.Model.Commands;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;

namespace ChildrenPath.Platform.Alerts.Interfaces.Transform;

public static class CreateAlertCommandFromResourceAssembler
{
    public static CreateAlertCommand ToCommandFromResource(CreateAlertResource resource)
    {
        // Parseo seguro de Enums (si falla, usa un default)
        if (!Enum.TryParse<AlertType>(resource.Type, true, out var alertType))
            alertType = AlertType.Delay;
            
        if (!Enum.TryParse<AlertPriority>(resource.Priority, true, out var priority))
            priority = AlertPriority.Medium;

        return new CreateAlertCommand(resource.TripId, alertType, priority, resource.Message);
    }
}