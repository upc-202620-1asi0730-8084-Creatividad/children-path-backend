using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Drivers.Domain;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;

namespace ChildrenPath.Platform.Drivers.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class DriversController : ControllerBase
{
    private static readonly List<Driver> _mockDrivers = new()
    {
        new Driver 
        { 
            Id = 1, Code = "DRV-001", FullName = "Carlos Pérez", Phone = "+51 987 221 104", Email = "carlos.perez@childrenpath.pe",
            LicenseNumber = "AIIb-458721", LicenseExpiry = new DateTime(2027, 4, 15), Status = DriverStatus.OnRoute,
            SafetyScore = 96, PunctualityScore = 92,
            Availabilities = new List<DriverAvailability> 
            { 
                new DriverAvailability { DayOfWeek = "Monday", StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(14, 0, 0) } 
            },
            Assignments = new List<DriverAssignment> 
            { 
                new DriverAssignment { VehicleId = 1, RouteId = 1, AssignedAt = DateTime.UtcNow } 
            }
        },
        new Driver 
        { 
            Id = 2, Code = "DRV-002", FullName = "María Gómez", Phone = "+51 955 420 771", Email = "maria.gomez@childrenpath.pe",
            LicenseNumber = "AIIb-884213", LicenseExpiry = new DateTime(2026, 12, 2), Status = DriverStatus.Available,
            SafetyScore = 94, PunctualityScore = 89,
            Availabilities = new List<DriverAvailability>(),
            Assignments = new List<DriverAssignment>()
        }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockDrivers);

    [HttpPost]
    public IActionResult Create([FromBody] CreateDriverResource resource)
    {
        var newId = _mockDrivers.Any() ? _mockDrivers.Max(d => d.Id) + 1 : 1;

        if (!Enum.TryParse<DriverStatus>(resource.Status, true, out var status))
            status = DriverStatus.Available;

        var driver = new Driver
        {
            Id = newId,
            Code = $"DRV-{newId:D3}",
            FullName = resource.FullName,
            Phone = resource.Phone,
            Email = resource.Email,
            LicenseNumber = resource.LicenseNumber,
            LicenseExpiry = resource.LicenseExpiry,
            Status = status,
            SafetyScore = resource.SafetyScore,
            PunctualityScore = resource.PunctualityScore
        };

        _mockDrivers.Add(driver);
        return CreatedAtAction(nameof(GetAll), new { id = driver.Id }, driver);
    }
}