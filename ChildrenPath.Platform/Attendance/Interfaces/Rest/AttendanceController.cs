using ChildrenPath.Platform.Attendance.Application.CommandServices;
using ChildrenPath.Platform.Attendance.Application.QueryServices;
using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;
using ChildrenPath.Platform.Attendance.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Attendance.Interfaces.Rest;

/// <summary>
///     REST API controller for managing student attendance records and dashboard views.
/// </summary>
/// <param name="attendanceQueryService">The query service used to retrieve attendance records.</param>
/// <param name="attendanceCommandService">The command service used to mutate attendance records.</param>
[ApiController]
[Route("api/v1/[controller]")]
public class AttendanceController(
    IAttendanceQueryService attendanceQueryService,
    IAttendanceCommandService attendanceCommandService)
    : ControllerBase
{
    /// <summary>
    ///     Retrieves all attendance records.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="AttendanceResource" /> entries.</returns>
    [HttpGet]
    [EndpointSummary("Get all attendance records")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAllAttendances(CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken);
        var resources = attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a single attendance record by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the attendance record.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching <see cref="AttendanceResource" />, or 404 if not found.</returns>
    [HttpGet("{id:int}")]
    [EndpointSummary("Get an attendance record by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttendanceById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var attendance = await attendanceQueryService.Handle(new GetAttendanceByIdQuery(id), cancellationToken);
        if (attendance is null) return NotFound();

        var resource = AttendanceResourceFromEntityAssembler.ToResourceFromEntity(attendance);
        return Ok(resource);
    }

    /// <summary>
    ///     Retrieves all attendance records associated with a specific trip.
    /// </summary>
    /// <param name="tripId">The unique identifier of the trip.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="AttendanceResource" /> entries for the trip.</returns>
    [HttpGet("trip/{tripId:int}")]
    [EndpointSummary("Get attendance records by trip id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAttendancesByTripId([FromRoute] int tripId, CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(
            new GetAttendancesByTripIdQuery(tripId),
            cancellationToken);

        var resources = attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves all attendance records associated with a specific student.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="AttendanceResource" /> entries for the student.</returns>
    [HttpGet("student/{studentId:int}")]
    [EndpointSummary("Get attendance records by student id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceResource>))]
    public async Task<IActionResult> GetAttendancesByStudentId(
        [FromRoute] int studentId,
        CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(
            new GetAttendancesByStudentIdQuery(studentId),
            cancellationToken);

        var resources = attendances.Select(AttendanceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Creates a new attendance record.
    /// </summary>
    /// <param name="resource">The attendance creation payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="AttendanceResource" /> with HTTP 201.</returns>
    [HttpPost]
    [EndpointSummary("Create a new attendance record")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAttendance(
        [FromBody] CreateAttendanceResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAttendanceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return AttendanceActionResultAssembler.ToActionResult(
            this,
            result,
            created => CreatedAtAction(
                nameof(GetAttendanceById),
                new { id = created.Id },
                AttendanceResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    /// <summary>
    ///     Updates the attendance status of an existing record.
    /// </summary>
    /// <param name="id">The unique identifier of the attendance record.</param>
    /// <param name="resource">The status update payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The updated <see cref="AttendanceResource" />.</returns>
    [HttpPatch("{id:int}/status")]
    [EndpointSummary("Update an attendance record status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAttendanceStatus(
        [FromRoute] int id,
        [FromBody] UpdateAttendanceStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateAttendanceStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return AttendanceActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AttendanceResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Marks an existing attendance record as dropped off at the destination.
    /// </summary>
    /// <param name="id">The unique identifier of the attendance record.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The updated <see cref="AttendanceResource" />.</returns>
    [HttpPatch("{id:int}/drop-off")]
    [EndpointSummary("Mark an attendance record as dropped off")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkDropOff([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await attendanceCommandService.Handle(new MarkDropOffCommand(id), cancellationToken);

        return AttendanceActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AttendanceResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Retrieves the aggregated attendance dashboard payload consumed by the Vue frontend.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="AttendanceDashboardResource" /> with summary KPIs, records, and timeline activities.</returns>
    [HttpGet("/api/v1/attendanceDashboard")]
    [EndpointSummary("Get Attendance Dashboard for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken);
        var dashboard = AttendanceDashboardAssembler.ToDashboardResource(attendances);
        return Ok(dashboard);
    }

    /// <summary>
    ///     Retrieves all attendance records formatted for the Vue frontend roster.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of <see cref="AttendanceDashboardRecordResource" /> entries.</returns>
    [HttpGet("/api/v1/attendanceRecords")]
    [EndpointSummary("Get Attendance Records for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AttendanceDashboardRecordResource>))]
    public async Task<IActionResult> GetFrontendRecords(CancellationToken cancellationToken)
    {
        var attendances = await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken);
        var records = attendances.Select(AttendanceDashboardAssembler.ToDashboardRecordResource);
        return Ok(records);
    }

    /// <summary>
    ///     Updates an attendance record from the Vue frontend using either formatted (e.g., ATT-001) or numeric identifiers.
    /// </summary>
    /// <param name="id">The formatted or numeric attendance identifier.</param>
    /// <param name="resource">The status update payload sent by the frontend.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The updated <see cref="AttendanceDashboardRecordResource" />.</returns>
    [HttpPut("/api/v1/attendanceRecords/{id}")]
    [EndpointSummary("Update an Attendance Record from Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDashboardRecordResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFrontendRecord(
        [FromRoute] string id,
        [FromBody] UpdateAttendanceStatusResource resource,
        CancellationToken cancellationToken)
    {
        if (!TryParseAttendanceId(id, out var attendanceId))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Attendance.InvalidId",
                detail: $"The attendance identifier '{id}' is not valid.");
        }

        var command = UpdateAttendanceStatusCommandFromResourceAssembler.ToCommandFromResource(attendanceId, resource);
        var result = await attendanceCommandService.Handle(command, cancellationToken);

        return AttendanceActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AttendanceDashboardAssembler.ToDashboardRecordResource(updated)));
    }

    private static bool TryParseAttendanceId(string rawId, out int attendanceId)
    {
        var cleaned = rawId.Trim();
        if (cleaned.StartsWith("ATT-", StringComparison.OrdinalIgnoreCase))
            cleaned = cleaned[4..];

        return int.TryParse(cleaned, out attendanceId);
    }
}