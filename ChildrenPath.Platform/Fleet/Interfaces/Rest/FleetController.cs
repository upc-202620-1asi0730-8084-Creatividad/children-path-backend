using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Fleet.Domain;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class FleetController : ControllerBase
{
    private static readonly List<Vehicle> _mockVehicles = new()
    {
        new Vehicle { Id = 1, Plate = "KW-204", Code = "KW-204", Brand = "Hyundai", Model = "H1 School Van", Year = 2022, Capacity = 18, AssignedStudents = 16, DriverName = "Carlos Pérez", RouteName = "Miraflores School Route", Status = "onRoute" },
        new Vehicle { Id = 2, Plate = "KW-118", Code = "KW-118", Brand = "Toyota", Model = "Hiace", Year = 2021, Capacity = 15, AssignedStudents = 13, DriverName = "María Gómez", RouteName = "San Isidro Morning Route", Status = "available" }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_mockVehicles);
    }
}