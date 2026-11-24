using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;

namespace ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;

public class Vehicle
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Capacity { get; set; }
    public int AssignedStudents { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public string Status { get; set; } = "available";
    public string EnergyType { get; set; } = "gasoline";
    public string OwnershipType { get; set; } = "company";
    public DateTime? NextMaintenanceDate { get; set; }
    public DateTime? LastInspectionDate { get; set; }
    public int MileageKm { get; set; }
    public int AvailabilityScore { get; set; } = 100;
    public bool DocumentsValid { get; set; } = true;
    public int? FleetId { get; set; }

    public TransportFleet? Fleet { get; set; }
    public ICollection<VehicleDocument> Documents { get; set; } = new List<VehicleDocument>();
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();

    public Vehicle()
    {
    }

    public Vehicle(CreateVehicleCommand command)
    {
        Plate = command.Plate;
        Code = command.Code;
        Brand = command.Brand;
        Model = command.Model;
        Year = command.Year;
        Capacity = command.Capacity;
        AssignedStudents = command.AssignedStudents;
        DriverName = command.DriverName;
        RouteName = command.RouteName;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "available" : command.Status;
        EnergyType = string.IsNullOrWhiteSpace(command.EnergyType) ? "gasoline" : command.EnergyType;
        OwnershipType = string.IsNullOrWhiteSpace(command.OwnershipType) ? "company" : command.OwnershipType;
        NextMaintenanceDate = command.NextMaintenanceDate;
        LastInspectionDate = command.LastInspectionDate;
        MileageKm = command.MileageKm;
        AvailabilityScore = command.AvailabilityScore > 0 ? command.AvailabilityScore : 100;
        DocumentsValid = command.DocumentsValid;
        FleetId = command.FleetId;
    }

    public void Update(UpdateVehicleCommand command)
    {
        Plate = command.Plate;
        Code = command.Code;
        Brand = command.Brand;
        Model = command.Model;
        Year = command.Year;
        Capacity = command.Capacity;
        AssignedStudents = command.AssignedStudents;
        DriverName = command.DriverName;
        RouteName = command.RouteName;
        Status = command.Status;
        EnergyType = command.EnergyType;
        OwnershipType = command.OwnershipType;
        NextMaintenanceDate = command.NextMaintenanceDate;
        LastInspectionDate = command.LastInspectionDate;
        MileageKm = command.MileageKm;
        AvailabilityScore = command.AvailabilityScore;
        DocumentsValid = command.DocumentsValid;
        FleetId = command.FleetId;
    }
}
