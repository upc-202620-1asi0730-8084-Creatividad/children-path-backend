using ChildrenPath.Platform.Companies.Domain.Model.Commands;

namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public class CompanyMember
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public DateTime? LastAccess { get; set; }
    public string Scope { get; set; } = string.Empty;

    public CompanyMember()
    {
    }

    public CompanyMember(CreateCompanyMemberCommand command)
    {
        CompanyId = command.CompanyId;
        Name = command.Name;
        Email = command.Email;
        Role = command.Role;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "active" : command.Status;
        LastAccess = command.LastAccess;
        Scope = command.Scope;
    }
}
