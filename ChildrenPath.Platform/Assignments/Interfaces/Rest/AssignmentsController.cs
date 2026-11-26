using ChildrenPath.Platform.Assignments.Application.CommandServices;
using ChildrenPath.Platform.Assignments.Application.QueryServices;
using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Domain.Model.Queries;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;
using ChildrenPath.Platform.Assignments.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Assignments.Interfaces.Rest;

/// <summary>
///     REST API controller for managing student transport assignments and frontend dashboard views.
/// </summary>
/// <param name="assignmentQueryService">The query service used to retrieve assignments.</param>
/// <param name="assignmentCommandService">The command service used to mutate assignments.</param>
[ApiController]
[Route("api/v1/[controller]")]
public class AssignmentsController(
    IAssignmentQueryService assignmentQueryService,
    IAssignmentCommandService assignmentCommandService)
    : ControllerBase
{
    /// <summary>
    ///     Retrieves all assignment records.
    /// </summary>
    [HttpGet]
    [EndpointSummary("Get all assignments")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    public async Task<IActionResult> GetAllAssignments(CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(new GetAllAssignmentsQuery(), cancellationToken);
        var resources = assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a single assignment record by its numeric identifier.
    /// </summary>
    [HttpGet("{id:int}")]
    [EndpointSummary("Get an assignment by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignmentById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var assignment = await assignmentQueryService.Handle(new GetAssignmentByIdQuery(id), cancellationToken);
        if (assignment is null) return NotFound();

        return Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment));
    }

    /// <summary>
    ///     Retrieves all assignments matching a specific status validated, pending, conflict, inactive.
    /// </summary>
    [HttpGet("status/{status}")]
    [EndpointSummary("Get assignments by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAssignmentsByStatus(
        [FromRoute] string status,
        CancellationToken cancellationToken)
    {
        var parsedStatus = AssignmentEnumMapper.ParseStatus(status);
        if (!Enum.IsDefined(parsedStatus))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Assignments.InvalidStatus",
                detail: $"The assignment status '{status}' is not valid.");
        }

        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByStatusQuery(parsedStatus),
            cancellationToken);

        var resources = assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves all assignments scheduled for a specific shift morning, afternoon.
    /// </summary>
    [HttpGet("shift/{shift}")]
    [EndpointSummary("Get assignments by shift")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAssignmentsByShift(
        [FromRoute] string shift,
        CancellationToken cancellationToken)
    {
        var parsedShift = AssignmentEnumMapper.ParseShift(shift);
        if (!Enum.IsDefined(parsedShift))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Assignments.InvalidShift",
                detail: $"The assignment shift '{shift}' is not valid.");
        }

        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByShiftQuery(parsedShift),
            cancellationToken);

        var resources = assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves all assignments associated with a specific student code (e.g., ST-001).
    /// </summary>
    [HttpGet("student/{studentCode}")]
    [EndpointSummary("Get assignments by student code")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    public async Task<IActionResult> GetAssignmentsByStudentCode(
        [FromRoute] string studentCode,
        CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByStudentCodeQuery(studentCode),
            cancellationToken);

        var resources = assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Creates a new assignment record.
    /// </summary>
    [HttpPost]
    [EndpointSummary("Create a new assignment")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAssignment(
        [FromBody] CreateAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAssignmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => CreatedAtAction(
                nameof(GetAssignmentById),
                new { id = created.Id },
                AssignmentResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    /// <summary>
    ///     Updates an existing assignment record.
    /// </summary>
    [HttpPut("{id:int}")]
    [EndpointSummary("Update an assignment")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAssignment(
        [FromRoute] int id,
        [FromBody] UpdateAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateAssignmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Marks an existing assignment as validated and ready.
    /// </summary>
    [HttpPatch("{id:int}/validate")]
    [EndpointSummary("Validate an assignment")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateAssignment([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await assignmentCommandService.Handle(new ValidateAssignmentCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Flags an existing assignment as having an operational conflict.
    /// </summary>
    [HttpPatch("{id:int}/conflict")]
    [EndpointSummary("Mark an assignment as conflict")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAssignmentConflict(
        [FromRoute] int id,
        [FromBody] MarkAssignmentConflictResource resource,
        CancellationToken cancellationToken)
    {
        var command = MarkAssignmentConflictCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Deletes an existing assignment record by its numeric identifier.
    /// </summary>
    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete an assignment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssignment([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await assignmentCommandService.Handle(new DeleteAssignmentCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }

    // =========================================================================
    // FRONTEND ENDPOINTS (Vue .env)
    // =========================================================================

    /// <summary>
    ///     Retrieves the aggregated assignments dashboard payload consumed by the Vue frontend.
    /// </summary>
    [HttpGet("/api/v1/assignmentDashboard")]
    [EndpointSummary("Get Assignment Dashboard for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(new GetAllAssignmentsQuery(), cancellationToken);
        var dashboard = AssignmentDashboardAssembler.ToDashboardResource(assignments);
        return Ok(dashboard);
    }

    /// <summary>
    ///     Retrieves all assignment records formatted for the Vue frontend.
    /// </summary>
    [HttpGet("/api/v1/assignmentRecords")]
    [EndpointSummary("Get Assignment Records for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentDashboardRecordResource>))]
    public async Task<IActionResult> GetFrontendRecords(CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(new GetAllAssignmentsQuery(), cancellationToken);
        var records = assignments.Select(AssignmentDashboardAssembler.ToDashboardRecordResource);
        return Ok(records);
    }

    /// <summary>
    ///     Creates an assignment record from the Vue frontend and returns it in dashboard format.
    /// </summary>
    [HttpPost("/api/v1/assignmentRecords")]
    [EndpointSummary("Create an Assignment Record from Frontend")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AssignmentDashboardRecordResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateFrontendRecord(
        [FromBody] CreateAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAssignmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created =>
            {
                var record = AssignmentDashboardAssembler.ToDashboardRecordResource(created);
                return Created($"/api/v1/assignmentRecords/{record.Id}", record);
            });
    }

    /// <summary>
    ///     Updates an assignment record from the Vue frontend using formatted (e.g., AS-001) or numeric identifiers.
    /// </summary>
    [HttpPut("/api/v1/assignmentRecords/{id}")]
    [EndpointSummary("Update an Assignment Record from Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentDashboardRecordResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateFrontendRecord(
        [FromRoute] string id,
        [FromBody] UpdateAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        if (!TryParseAssignmentId(id, out var assignmentId))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Assignments.InvalidId",
                detail: $"The assignment identifier '{id}' is not valid.");
        }

        var command = UpdateAssignmentCommandFromResourceAssembler.ToCommandFromResource(assignmentId, resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentDashboardAssembler.ToDashboardRecordResource(updated)));
    }

    /// <summary>
    ///     Deletes an assignment record from the Vue frontend using formatted  or numeric identifiers.
    /// </summary>
    [HttpDelete("/api/v1/assignmentRecords/{id}")]
    [EndpointSummary("Delete an Assignment Record from Frontend")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFrontendRecord(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        if (!TryParseAssignmentId(id, out var assignmentId))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Assignments.InvalidId",
                detail: $"The assignment identifier '{id}' is not valid.");
        }

        var result = await assignmentCommandService.Handle(new DeleteAssignmentCommand(assignmentId), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }

    private static bool TryParseAssignmentId(string rawId, out int assignmentId)
    {
        var cleaned = rawId.Trim();
        if (cleaned.StartsWith("AS-", StringComparison.OrdinalIgnoreCase))
            cleaned = cleaned[3..];

        return int.TryParse(cleaned, out assignmentId) && assignmentId > 0;
    }
}