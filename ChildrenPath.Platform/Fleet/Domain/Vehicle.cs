namespace ChildrenPath.Platform.Fleet.Domain;

public enum VehicleStatus { Active, InMaintenance, Inactive, Retired }

public class VehicleDocument
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public int VehicleId { get; set; }
    public bool Validate() => ExpiryDate > DateTime.UtcNow;
}

public class MaintenanceRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int VehicleId { get; set; }
}

public class Vehicle
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Active;
    
    // Campos extra útiles para tu UI
    public string DriverName { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public int AssignedStudents { get; set; }

    // Composición
    public List<VehicleDocument> Documents { get; set; } = new();
    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();

    public void UpdateStatus(VehicleStatus newStatus) => Status = newStatus;
}