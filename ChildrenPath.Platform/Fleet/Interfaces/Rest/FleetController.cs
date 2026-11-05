using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Fleet.Interfaces.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class FleetController : ControllerBase
{
    private static readonly List<Vehicle> _mockVehicles = new()
    {
        new Vehicle 
        { 
            Id = 1, PlateNumber = "KW-204", Brand = "Hyundai", Model = "H1 School Van", Capacity = 18, 
            AssignedStudents = 16, DriverName = "Carlos Pérez", RouteName = "Miraflores", Status = VehicleStatus.Active,
            FleetId = 1, Documents = new List<VehicleDocument> { new VehicleDocument { Type = "SOAT", ExpiryDate = DateTime.UtcNow.AddMonths(6) } }
        },
        new Vehicle 
        { 
            Id = 2, PlateNumber = "KW-076", Brand = "Mercedes", Model = "Sprinter", Capacity = 22, 
            AssignedStudents = 20, DriverName = "Luis Torres", RouteName = "Surco", Status = VehicleStatus.InMaintenance,
            FleetId = null // Vehículo independiente
        }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockVehicles);

    [HttpPost]
    public IActionResult Create([FromBody] CreateVehicleResource resource)
    {
        var newId = _mockVehicles.Any() ? _mockVehicles.Max(v => v.Id) + 1 : 1;

        var vehicle = new Vehicle
        {
            Id = newId,
            PlateNumber = resource.PlateNumber,
            Brand = resource.Brand,
            Model = resource.Model,
            Capacity = resource.Capacity,
            DriverName = resource.DriverName,
            RouteName = resource.RouteName,
            AssignedStudents = resource.AssignedStudents,
            Status = VehicleStatus.Active,
            FleetId = resource.FleetId
        };

        _mockVehicles.Add(vehicle);
        return CreatedAtAction(nameof(GetAll), new { id = vehicle.Id }, vehicle);
    }
}