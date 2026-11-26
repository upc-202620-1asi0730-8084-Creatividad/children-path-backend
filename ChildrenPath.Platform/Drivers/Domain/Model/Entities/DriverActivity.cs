namespace ChildrenPath.Platform.Drivers.Domain.Model.Entities;

public class DriverActivity
{
    public int Id { get; set; }
    public int? DriverId { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "completed";
}