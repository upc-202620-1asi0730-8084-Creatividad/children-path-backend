using ChildrenPath.Platform.Attendance.Application.CommandServices;
using ChildrenPath.Platform.Attendance.Application.QueryServices;
using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;
using ChildrenPath.Platform.Attendance.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Model;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Attendance.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AttendanceController(
    IAttendanceQueryService attendanceQueryService,
    IAttendanceCommandService attendanceCommandService,
    IAttendanceDashboardQueryService attendanceDashboardQueryService)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("get all attendance records")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAllAttendances(CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken);
        return Ok(attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get an attendance record by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttendanceById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var attendance = await attendanceQueryService.Handle(new GetAttendanceByIdQuery(id), cancellationToken);
        if (attendance is null) return NotFound();

        return Ok(AttendanceResourceFromEntityAssembler.ToResourceFromEntity(attendance));
    }

    [HttpGet("trip/{tripId:int}")]
    [EndpointSummary("get attendance records by trip id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAttendancesByTripId([FromRoute] int tripId, CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAttendancesByTripIdQuery(tripId), cancellationToken);
        return Ok(attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("student/{studentId:int}")]
    [EndpointSummary("get attendance records by student id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAttendancesByStudentId([FromRoute] int studentId, CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAttendancesByStudentIdQuery(studentId), cancellationToken);
        return Ok(attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("create a new attendance record")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceResource resource, CancellationToken cancellationToken)
    {
        var command = CreateAttendanceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, created =>
            CreatedAtAction(nameof(GetAttendanceById), new { id = created.Id },
                AttendanceResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPatch("{id:int}/status")]
    [EndpointSummary("update an attendance record status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAttendanceStatus([FromRoute] int id, [FromBody] UpdateAttendanceStatusResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateAttendanceStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, updated =>
            Ok(AttendanceResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPatch("{id:int}/drop-off")]
    [EndpointSummary("mark an attendance record as dropped off")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkDropOff([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await attendanceCommandService.Handle(new MarkDropOffCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, updated =>
            Ok(AttendanceResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpGet("dashboard")]
    [EndpointSummary("get attendance dashboard for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await attendanceDashboardQueryService.Handle(new GetAttendanceDashboardQuery(), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this, result, 
            snapshot => Ok(AttendanceDashboardAssembler.ToDashboardResource(snapshot)));
    }

    [HttpGet("records")]
    [EndpointSummary("get attendance records for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceDashboardRecordResource>))]
    public async Task<IActionResult> GetFrontendRecords(CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken);
        return Ok(attendances.Select(AttendanceDashboardAssembler.ToDashboardRecordResource));
    }

    [HttpPut("records/{id}")]
    [EndpointSummary("update an attendance record from frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDashboardRecordResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFrontendRecord([FromRoute] string id, [FromBody] UpdateAttendanceStatusResource resource, CancellationToken cancellationToken)
    {
        if (!AttendanceIdParser.TryParse(id, out var attendanceId))
        {
            return ResultActionResultAssembler.ToActionResult(this,
                Result<object>.Failure(Error.Validation("Attendance.InvalidId", $"The attendance identifier '{id}' is not valid.")),
                _ => Ok());
        }

        var command = UpdateAttendanceStatusCommandFromResourceAssembler.ToCommandFromResource(attendanceId, resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, updated =>
            Ok(AttendanceDashboardAssembler.ToDashboardRecordResource(updated)));
    }
}