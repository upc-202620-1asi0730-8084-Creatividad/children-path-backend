using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Attendance.Domain;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;

using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Attendance; 

namespace ChildrenPath.Platform.Attendance.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AttendanceController : ControllerBase
{
    private static readonly List<AttendanceEntity> _mockAttendances = new()
    {
        new AttendanceEntity 
        { 
            Id = 1, StudentId = 1, TripId = 101, Status = AttendanceStatus.DroppedOff,
            BoardingRecord = new BoardingRecord { Location = "Calle Pino 42", BoardedAt = DateTime.UtcNow.AddHours(-1) }
        },
        new AttendanceEntity 
        { 
            Id = 2, StudentId = 3, TripId = 101, Status = AttendanceStatus.Boarded,
            BoardingRecord = new BoardingRecord { Location = "Javier Prado checkpoint", BoardedAt = DateTime.UtcNow.AddMinutes(-30) }
        },
        new AttendanceEntity 
        { 
            Id = 3, StudentId = 5, TripId = 101, Status = AttendanceStatus.Absent,
            AbsenceRecord = new AbsenceRecord { Reason = "Medical appointment", Justified = true }
        }
    };

    [HttpGet]
    public IActionResult GetAll() 
    {
        var response = _mockAttendances.Select(a => new
        {
            id = $"ATT-{a.Id:D3}",
            studentCode = $"ST-{a.StudentId:D3}",
            studentName = a.StudentId == 1 ? "María Vega" : a.StudentId == 3 ? "Valeria Cruz" : "Ana Méndez",
            grade = a.StudentId == 1 ? "4th Grade" : a.StudentId == 3 ? "1st Secondary" : "4th Grade",
            routeName = "Miraflores School Route",
            vehiclePlate = "KW-204",
            driverName = "Carlos Pérez",
            pickupPoint = "Calle Pino 42",
            school = "Colegio San Agustín",
            estimatedArrival = "07:15",
            checkInTime = a.BoardingRecord?.BoardedAt.ToString("HH:mm") ?? null,
            dropOffTime = a.Status == AttendanceStatus.DroppedOff ? "07:14" : null,
            status = a.Status switch
            {
                AttendanceStatus.Boarded => "on_board",
                AttendanceStatus.DroppedOff => "arrived",
                AttendanceStatus.Absent => "absent",
                _ => "waiting"
            },
            reliability = a.Status == AttendanceStatus.DroppedOff ? 100 : 82,
            lastEvent = a.Status.ToString(),
            guardianName = "María López",
            notes = "Student arrived safely at school."
        });

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAttendanceResource resource)
    {
        var newId = _mockAttendances.Any() ? _mockAttendances.Max(a => a.Id) + 1 : 1;

        var attendance = new AttendanceEntity
        {
            Id = newId,
            StudentId = resource.StudentId,
            TripId = resource.TripId,
            Timestamp = DateTime.UtcNow
        };

        if (!string.IsNullOrEmpty(resource.AbsenceReason))
        {
            attendance.MarkAbsent(resource.AbsenceReason, resource.IsAbsenceJustified);
        }
        else
        {
            attendance.MarkBoarding(resource.Location);
        }

        _mockAttendances.Add(attendance);
        return CreatedAtAction(nameof(GetAll), new { id = attendance.Id }, attendance);
    }

    [HttpPatch("{id:int}/drop-off")]
    public IActionResult DropOff(int id)
    {
        var attendance = _mockAttendances.FirstOrDefault(a => a.Id == id);
        if (attendance == null) return NotFound();

        attendance.MarkDropOff();
        return Ok(attendance);
    }
}