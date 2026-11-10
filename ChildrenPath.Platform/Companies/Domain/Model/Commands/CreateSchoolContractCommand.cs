namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record CreateSchoolContractCommand(
    int CompanyId,
    string SchoolName,
    string District,
    int RouteCount,
    int StudentCount,
    string ContactName,
    DateTime RenewalDate,
    string Status,
    int Score,
    string Notes
);
