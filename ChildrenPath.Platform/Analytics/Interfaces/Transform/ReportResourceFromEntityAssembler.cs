using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;

namespace ChildrenPath.Platform.Analytics.Interfaces.Transform;

public static class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report entity)
    {
        return new ReportResource(
            Id: $"r{entity.Id}",
            Type: entity.Type.ToString().ToLower(),
            Title: $"{entity.Type} report",
            Period: entity.Period,
            GeneratedAt: entity.GeneratedAt,
            Owner: "Company Admin",
            Status: "ready",
            Records: entity.Metrics.FirstOrDefault()?.Value ?? 0
        );
    }
}