using ChildrenPath.Platform.Analytics.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Analytics.Domain.Model.Events;

public class ReportGeneratedEvent(int reportId, ReportType type) : IEvent
{
    public int ReportId { get; } = reportId;
    public ReportType Type { get; } = type;
}