using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Fleet.Interfaces.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")] 
public class FleetController : ControllerBase
{
    // BD antes de crear la real
    private static readonly List<Vehicle> _mockVehicles = new()
    {
        new Vehicle { Id = 1, Plate = "KW-204", Code = "KW-204", Brand = "Hyundai", Model = "H1 School Van", Year = 2022, Capacity = 18, AssignedStudents = 16, DriverName = "Carlos Pérez", RouteName = "Miraflores School Route", Status = "onRoute" },
        new Vehicle { Id = 2, Plate = "KW-118", Code = "KW-118", Brand = "Toyota", Model = "Hiace", Year = 2021, Capacity = 15, AssignedStudents = 13, DriverName = "María Gómez", RouteName = "San Isidro Morning Route", Status = "available" }
    };

    // 1. GET: Obtener todos
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_mockVehicles);
    }

    // 2. POST: Crear un vehículo
    [HttpPost]
    public IActionResult Create([FromBody] CreateVehicleResource resource)
    {
        var newId = _mockVehicles.Any() ? _mockVehicles.Max(v => v.Id) + 1 : 1;

        var vehicle = new Vehicle
        {
            Id = newId,
            Plate = resource.Plate,
            Code = resource.Code,
            Brand = resource.Brand,
            Model = resource.Model,
            Year = resource.Year,
            Capacity = resource.Capacity,
            DriverName = resource.DriverName,
            RouteName = resource.RouteName,
            AssignedStudents = resource.AssignedStudents,
            Status = resource.Status
        };

        _mockVehicles.Add(vehicle);

        return CreatedAtAction(nameof(GetAll), new { id = vehicle.Id }, vehicle);
    }

    // 3. PUT: Actualizar un vehículo existente
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateVehicleResource resource)
    {
        var existingVehicle = _mockVehicles.FirstOrDefault(v => v.Id == id);
        if (existingVehicle == null) return NotFound("Vehículo no encontrado");

        existingVehicle.Plate = resource.Plate;
        existingVehicle.Code = resource.Code;
        existingVehicle.Brand = resource.Brand;
        existingVehicle.Model = resource.Model;
        existingVehicle.Year = resource.Year;
        existingVehicle.Capacity = resource.Capacity;
        existingVehicle.DriverName = resource.DriverName;
        existingVehicle.RouteName = resource.RouteName;
        existingVehicle.AssignedStudents = resource.AssignedStudents;
        existingVehicle.Status = resource.Status;

        return Ok(existingVehicle);
    }

    // 4. DELETE: Eliminar un vehículo
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var vehicle = _mockVehicles.FirstOrDefault(v => v.Id == id);
        if (vehicle == null) return NotFound("Vehículo no encontrado");

        _mockVehicles.Remove(vehicle);
        return NoContent();
    }
}
