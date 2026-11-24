namespace ChildrenPath.Platform.Fleet.Domain.Model.Entities;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public int MileageAtService { get; set; }
}
