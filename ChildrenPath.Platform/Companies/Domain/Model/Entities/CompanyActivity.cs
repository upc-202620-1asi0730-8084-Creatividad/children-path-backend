using ChildrenPath.Platform.Companies.Domain.Model.Commands;

namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public class CompanyActivity
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public DateTime Time { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "completed";

    public CompanyActivity()
    {
    }

    public CompanyActivity(CreateCompanyActivityCommand command)
    {
        CompanyId = command.CompanyId;
        Time = command.Time == default ? DateTime.UtcNow : command.Time;
        Title = command.Title;
        Description = command.Description;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "completed" : command.Status;
    }
}
