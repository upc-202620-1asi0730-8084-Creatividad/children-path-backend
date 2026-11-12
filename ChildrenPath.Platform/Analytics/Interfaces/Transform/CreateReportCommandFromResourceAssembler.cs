using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Analytics.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;

namespace ChildrenPath.Platform.Analytics.Interfaces.Transform;

public static class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        if (!Enum.TryParse<ReportType>(resource.Type, true, out var reportType))
            reportType = ReportType.Performance;

        return new CreateReportCommand(reportType, resource.Period);
    }
}