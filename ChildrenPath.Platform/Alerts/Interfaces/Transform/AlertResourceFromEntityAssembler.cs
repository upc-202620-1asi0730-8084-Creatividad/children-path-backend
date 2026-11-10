using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;

namespace ChildrenPath.Platform.Alerts.Interfaces.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity)
    {
        return new AlertResource(
            entity.Id,
            entity.TripId,
            entity.Type.ToString(),
            entity.Priority.ToString(),
            entity.Message,
            entity.Status.ToString(),
            entity.CreatedAt,
            entity.Notifications.Select(n => new NotificationResource(
                n.Id,
                n.Channel.ToString(),
                n.Recipient,
                n.Status.ToString(),
                n.SentAt
            )).ToList()
        );
    }
}