namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record CreateCompanyActivityCommand(
    int CompanyId,
    DateTime Time,
    string Title,
    string Description,
    string Status
);
