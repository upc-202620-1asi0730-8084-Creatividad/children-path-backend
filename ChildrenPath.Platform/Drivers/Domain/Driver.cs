namespace ChildrenPath.Platform.Drivers.Domain;

// --- ENUM ---
public enum DriverStatus { Available, OnRoute, OffDuty, Suspended }

// --- ENTIDADES COMPUESTAS ---
public class DriverAvailability
{
    public int Id { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DriverId { get; set; } // FK

    public bool CheckAvailability(DateTime date)
    {
        return date.DayOfWeek.ToString() == DayOfWeek;
    }
}

public class DriverAssignment
{
    public int Id { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedAt { get; set; }
    
    // Referencias a otros Bounded Contexts (como indica tu diagrama)
    public int VehicleId { get; set; }
    public int RouteId { get; set; }
    public int DriverId { get; set; } // FK

    public void Unassign() => UnassignedAt = DateTime.UtcNow;
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class Driver
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Datos de Licencia (del UML)
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime LicenseExpiry { get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Available;
    
    // Datos para Frontend
    public int SafetyScore { get; set; }
    public int PunctualityScore { get; set; }

    // Composición (del UML)
    public List<DriverAvailability> Availabilities { get; set; } = new();
    public List<DriverAssignment> Assignments { get; set; } = new();

    // Método del UML
    public void UpdateAvailability(DriverStatus newStatus) => Status = newStatus;
}