using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Assignments.Domain;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AssignmentsController : ControllerBase
{
    private static readonly List<Assignment> _mockAssignments = new()
    {
        new Assignment 
        { 
            Id = 1, StudentCode = "ST-001", StudentName = "María Vega", Grade = "4th Grade", GuardianName = "María López",
            RouteCode = "RT-001", RouteName = "Miraflores School Route", VehiclePlate = "KW-204", DriverName = "Carlos Pérez",
            Shift = AssignmentShift.Morning, PickupPoint = "Calle Pino 42", PickupWindow = "06:45 - 06:55",
            CapacityUsage = 89, ValidationScore = 98, Validation = AssignmentValidation.Ready, 
            Status = AssignmentStatus.Validated, LastUpdated = DateTime.UtcNow.AddHours(-2),
            Notes = "Student assigned to active route and confirmed guardian record."
        },
        new Assignment 
        { 
            Id = 2, StudentCode = "ST-003", StudentName = "Valeria Cruz", Grade = "1st Secondary", GuardianName = "Elena Cruz",
            RouteCode = "RT-003", RouteName = "Surco Pickup Route", VehiclePlate = "KW-076", DriverName = "Luis Torres",
            Shift = AssignmentShift.Morning, PickupPoint = "Javier Prado checkpoint", PickupWindow = "07:05 - 07:15",
            CapacityUsage = 91, ValidationScore = 76, Validation = AssignmentValidation.ScheduleConflict, 
            Status = AssignmentStatus.Conflict, LastUpdated = DateTime.UtcNow.AddHours(-1),
            Notes = "Pickup window overlaps with another planned stop."
        }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockAssignments);

    [HttpPost]
    public IActionResult Create([FromBody] CreateAssignmentResource resource)
    {
        var newId = _mockAssignments.Any() ? _mockAssignments.Max(a => a.Id) + 1 : 1;

        if (!Enum.TryParse<AssignmentShift>(resource.Shift, true, out var shift))
            shift = AssignmentShift.Morning;

        var assignment = new Assignment
        {
            Id = newId,
            StudentCode = resource.StudentCode,
            StudentName = resource.StudentName,
            Grade = resource.Grade,
            GuardianName = resource.GuardianName,
            RouteCode = resource.RouteCode,
            RouteName = resource.RouteName,
            VehiclePlate = resource.VehiclePlate,
            DriverName = resource.DriverName,
            Shift = shift,
            PickupPoint = resource.PickupPoint,
            PickupWindow = resource.PickupWindow,
            CapacityUsage = resource.CapacityUsage,
            ValidationScore = 90, // Valor por defecto inicial
            Validation = AssignmentValidation.Ready,
            Status = AssignmentStatus.Pending, // Pendiente de validación
            Notes = resource.Notes,
            LastUpdated = DateTime.UtcNow
        };

        _mockAssignments.Add(assignment);
        return CreatedAtAction(nameof(GetAll), new { id = assignment.Id }, assignment);
    }
    
    // Endpoint extra para validar una asignación
    [HttpPatch("{id:int}/validate")]
    public IActionResult Validate(int id)
    {
        var assignment = _mockAssignments.FirstOrDefault(a => a.Id == id);
        if (assignment == null) return NotFound();

        assignment.ValidateAssignment();
        return Ok(assignment);
    }
}