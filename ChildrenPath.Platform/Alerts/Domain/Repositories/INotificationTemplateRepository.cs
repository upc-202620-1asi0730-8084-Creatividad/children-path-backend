using ChildrenPath.Platform.Alerts.Domain.Model.Entities;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Alerts.Domain.Repositories;

public interface INotificationTemplateRepository : IBaseRepository<NotificationTemplate>
{
    Task<NotificationTemplate?> FindByChannelAsync(NotificationChannel channel, CancellationToken ct = default);
}