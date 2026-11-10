namespace ChildrenPath.Platform.Fleet.Domain;

// --- ENUMS ---
public enum VehicleStatus { Active, InMaintenance, Inactive, Retired }

// --- ENTIDADES COMPUESTAS ---
public class VehicleDocument
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // SOAT, Revisión Técnica, etc.
    public DateTime ExpiryDate { get; set; }
    public int VehicleId { get; set; } // FK

    public bool Validate() => ExpiryDate > DateTime.UtcNow;
}

public class MaintenanceRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public int VehicleId { get; set; } // FK

    public void Register() { /* Lógica de registro */ }
}

// --- ENTIDAD PRINCIPAL ---
public class Vehicle
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Active;
    
    // Asociación: Un vehículo puede pertenecer a una flota o ser independiente (Null)
    public int? FleetId { get; set; } 
    public TransportFleet? Fleet { get; set; }

    // Asignación de conductor (del método assign(driverId))
    public int? DriverId { get; set; }
    public string DriverName { get; set; } = string.Empty; // Útil para el Frontend
    public string RouteName { get; set; } = string.Empty;  // Útil para el Frontend
    public int AssignedStudents { get; set; }              // Útil para el Frontend

    // Composición: Los documentos y mantenimientos se borran con el vehículo
    public List<VehicleDocument> Documents { get; set; } = new();
    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();

    // Métodos del UML
    public bool Assign(int driverId)
    {
        DriverId = driverId;
        return true;
    }

    public void UpdateStatus(VehicleStatus newStatus) => Status = newStatus;
}

// --- AGREGADO FLOTA ---
public class TransportFleet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Vehicle> Vehicles { get; set; } = new();

    public void AddVehicle(Vehicle vehicle)
    {
        Vehicles.Add(vehicle);
        vehicle.FleetId = this.Id;
    }

    public bool RemoveVehicle(int vehicleId)
    {
        var vehicle = Vehicles.FirstOrDefault(v => v.Id == vehicleId);
        if (vehicle == null) return false;
        
        Vehicles.Remove(vehicle);
        vehicle.FleetId = null;
        return true;
    }
}