using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Students.Domain;
using ChildrenPath.Platform.Students.Interfaces.Resources;

namespace ChildrenPath.Platform.Students.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<Student> _mockStudents = new()
    {
        new Student { 
            Id = 1, Code = "ST-001", FirstName = "María", LastName = "Vega", Grade = "4th Grade", SchoolName = "Lima Norte School",
            Status = "active", AttendanceRate = 98,
            Guardians = new List<Guardian> { new Guardian { FullName = "María López", Phone = "+51 951 111 222", Relationship = "Mother" } },
            PickupPoint = new PickupPoint { Address = "Calle Pino 42", Latitude = -12.1195, Longitude = -77.0308 }
        }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockStudents);

    [HttpPost]
    public IActionResult Create([FromBody] CreateStudentResource resource)
    {
        var newId = _mockStudents.Any() ? _mockStudents.Max(s => s.Id) + 1 : 1;

        var student = new Student
        {
            Id = newId,
            Code = $"ST-{newId:D3}",
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            Grade = resource.Grade,
            SchoolName = resource.SchoolName,
            Status = "active",
            Guardians = new List<Guardian>
            {
                new Guardian { FullName = resource.GuardianFullName, Phone = resource.GuardianPhone, Relationship = resource.GuardianRelationship }
            },
            PickupPoint = new PickupPoint { Address = resource.PickupAddress }
        };

        _mockStudents.Add(student);
        return CreatedAtAction(nameof(GetAll), new { id = student.Id }, student);
    }
}