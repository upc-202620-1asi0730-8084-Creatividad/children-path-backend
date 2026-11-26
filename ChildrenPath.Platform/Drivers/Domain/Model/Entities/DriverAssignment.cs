namespace ChildrenPath.Platform.Drivers.Domain.Model.Entities;

public class DriverAssignment
{
    public int Id { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedAt { get; set; }
    public int VehicleId { get; set; }
    public int RouteId { get; set; }
    public int DriverId { get; set; }

    public void Unassign() => UnassignedAt = DateTime.UtcNow;
}
