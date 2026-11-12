namespace ChildrenPath.Platform.Analytics.Interfaces.Resources;

public record ReportResource(
    string Id,
    string Type,
    string Title,
    string Period,
    DateTime GeneratedAt,
    string Owner,
    string Status,
    decimal Records
);