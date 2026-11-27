namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record CompanyActivityResource(
    int Id,
    int CompanyId,
    DateTime Time,
    string Title,
    string Description,
    string Status
);
