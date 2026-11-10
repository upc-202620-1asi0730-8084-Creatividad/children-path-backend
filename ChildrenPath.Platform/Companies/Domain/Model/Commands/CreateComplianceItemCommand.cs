namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record CreateComplianceItemCommand(
    int CompanyId,
    string Title,
    string Category,
    string Owner,
    DateTime DueDate,
    string Status,
    string Severity,
    string Description
);
