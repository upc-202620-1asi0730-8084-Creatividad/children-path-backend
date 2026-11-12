using ChildrenPath.Platform.Analytics.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Analytics.Domain.Model.Commands;

public record CreateReportCommand(ReportType Type, string Period);