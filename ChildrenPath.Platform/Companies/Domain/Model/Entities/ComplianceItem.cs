using ChildrenPath.Platform.Companies.Domain.Model.Commands;

namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public class ComplianceItem
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "pending";
    public string Severity { get; set; } = "low";
    public string Description { get; set; } = string.Empty;

    public ComplianceItem()
    {
    }

    public ComplianceItem(CreateComplianceItemCommand command)
    {
        CompanyId = command.CompanyId;
        Title = command.Title;
        Category = command.Category;
        Owner = command.Owner;
        DueDate = command.DueDate;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "pending" : command.Status;
        Severity = string.IsNullOrWhiteSpace(command.Severity) ? "low" : command.Severity;
        Description = command.Description;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
    }
}
