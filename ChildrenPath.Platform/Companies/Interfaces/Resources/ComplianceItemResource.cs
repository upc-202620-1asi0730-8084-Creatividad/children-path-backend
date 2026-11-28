namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record ComplianceItemResource(
    int Id,
    int CompanyId,
    string Title,
    string Category,
    string Owner,
    DateTime DueDate,
    string Status,
    string Severity,
    string Description
);
