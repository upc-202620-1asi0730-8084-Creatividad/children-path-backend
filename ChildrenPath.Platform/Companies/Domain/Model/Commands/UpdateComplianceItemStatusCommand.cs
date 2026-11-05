namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record UpdateComplianceItemStatusCommand(
    int Id,
    string Status
);
