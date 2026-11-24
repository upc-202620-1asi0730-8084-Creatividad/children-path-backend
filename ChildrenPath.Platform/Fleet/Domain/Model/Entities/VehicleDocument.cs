namespace ChildrenPath.Platform.Fleet.Domain.Model.Entities;

public class VehicleDocument
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsValid => ExpiryDate >= DateTime.UtcNow.Date;
}
