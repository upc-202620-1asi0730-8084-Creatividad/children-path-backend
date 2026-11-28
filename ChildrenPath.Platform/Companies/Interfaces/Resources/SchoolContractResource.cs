namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record SchoolContractResource(
    int Id,
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
