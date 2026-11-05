namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record CreateCompanyMemberCommand(
    int CompanyId,
    string Name,
    string Email,
    string Role,
    string Status,
    DateTime? LastAccess,
    string Scope
);
