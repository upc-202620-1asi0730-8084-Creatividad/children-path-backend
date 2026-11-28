using ChildrenPath.Platform.Assignments.Application.CommandServices;
using ChildrenPath.Platform.Assignments.Application.QueryServices;
using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Domain.Model.Errors;
using ChildrenPath.Platform.Assignments.Domain.Model.Queries;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;
using ChildrenPath.Platform.Assignments.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Assignments.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AssignmentsController(
    IAssignmentQueryService assignmentQueryService,
    IAssignmentCommandService assignmentCommandService,
    IAssignmentDashboardQueryService assignmentDashboardQueryService)
    : ControllerBase
{
    // ─────────────── CRUD estándar ───────────────

    [HttpGet]
    [EndpointSummary("get all assignments")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    public async Task<IActionResult> GetAllAssignments(CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(new GetAllAssignmentsQuery(), cancellationToken);
        return Ok(assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get an assignment by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignmentById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var assignment = await assignmentQueryService.Handle(new GetAssignmentByIdQuery(id), cancellationToken);
        if (assignment is null) return NotFound();
        return Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment));
    }

    [HttpGet("status/{status}")]
    [EndpointSummary("get assignments by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAssignmentsByStatus([FromRoute] string status, CancellationToken cancellationToken)
    {
        var parsedStatus = AssignmentEnumMapper.ParseStatus(status);
        if (!Enum.IsDefined(parsedStatus))
            return ResultActionResultAssembler.ToActionResult(this,
                Result<object>.Failure(AssignmentErrors.InvalidStatus),
                _ => Ok());

        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByStatusQuery(parsedStatus), cancellationToken);

        return Ok(assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("shift/{shift}")]
    [EndpointSummary("get assignments by shift")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAssignmentsByShift([FromRoute] string shift, CancellationToken cancellationToken)
    {
        var parsedShift = AssignmentEnumMapper.ParseShift(shift);
        if (!Enum.IsDefined(parsedShift))
            return ResultActionResultAssembler.ToActionResult(this,
                Result<object>.Failure(AssignmentErrors.InvalidShift),
                _ => Ok());

        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByShiftQuery(parsedShift), cancellationToken);

        return Ok(assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("student/{studentCode}")]
    [EndpointSummary("get assignments by student code")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AssignmentResource>))]
    public async Task<IActionResult> GetAssignmentsByStudentCode([FromRoute] string studentCode, CancellationToken cancellationToken)
    {
        var assignments = await assignmentQueryService.Handle(
            new GetAssignmentsByStudentCodeQuery(studentCode), cancellationToken);

        return Ok(assignments.Select(AssignmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("create a new assignment")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentResource resource, CancellationToken cancellationToken)
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

    [HttpPut("{id:int}")]
    [EndpointSummary("update an assignment")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAssignment([FromRoute] int id, [FromBody] UpdateAssignmentResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateAssignmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPatch("{id:int}/validate")]
    [EndpointSummary("validate an assignment")]
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

    [HttpPatch("{id:int}/conflict")]
    [EndpointSummary("mark an assignment as conflict")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAssignmentConflict([FromRoute] int id, [FromBody] MarkAssignmentConflictResource resource, CancellationToken cancellationToken)
    {
        var command = MarkAssignmentConflictCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await assignmentCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(AssignmentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("delete an assignment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssignment([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await assignmentCommandService.Handle(new DeleteAssignmentCommand(id), cancellationToken);
        return ResultActionResultAssembler.ToActionResult(this, result, _ => NoContent());
    }

    // ─────────────── Dashboard (formato frontend) ───────────────

    [HttpGet("dashboard")]
    [EndpointSummary("get assignments dashboard for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignmentDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await assignmentDashboardQueryService
            .Handle(new GetAssignmentDashboardQuery(), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            snapshot => Ok(AssignmentDashboardAssembler.ToDashboardResource(snapshot)));
    }
}