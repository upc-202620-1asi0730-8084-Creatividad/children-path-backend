using ChildrenPath.Platform.Companies.Domain.Model.Commands;

namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public class SchoolContract
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public int RouteCount { get; set; }
    public int StudentCount { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public DateTime RenewalDate { get; set; }
    public string Status { get; set; } = "active";
    public int Score { get; set; }
    public string Notes { get; set; } = string.Empty;

    public SchoolContract()
    {
    }

    public SchoolContract(CreateSchoolContractCommand command)
    {
        CompanyId = command.CompanyId;
        SchoolName = command.SchoolName;
        District = command.District;
        RouteCount = command.RouteCount;
        StudentCount = command.StudentCount;
        ContactName = command.ContactName;
        RenewalDate = command.RenewalDate;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "active" : command.Status;
        Score = command.Score;
        Notes = command.Notes;
    }
}
