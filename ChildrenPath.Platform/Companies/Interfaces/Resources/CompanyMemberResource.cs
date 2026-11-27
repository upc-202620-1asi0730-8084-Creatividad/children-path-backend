namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record CompanyMemberResource(
    int Id,
    int CompanyId,
    string Name,
    string Email,
    string Role,
    string Status,
    DateTime? LastAccess,
    string Scope
);
