using ChildrenPath.Platform.Shared.Domain.Model.Entities;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;

public partial class Alert : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}