using ChildrenPath.Platform.Shared.Domain.Model.Entities;

namespace ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;

public partial class TrackingSession : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}