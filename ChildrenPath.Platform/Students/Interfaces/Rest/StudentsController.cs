using System.Net.Mime;
using ChildrenPath.Platform.Students.Application.CommandServices;
using ChildrenPath.Platform.Students.Application.QueryServices;
using ChildrenPath.Platform.Students.Domain.Model.Commands;
using ChildrenPath.Platform.Students.Domain.Model.Queries;
using ChildrenPath.Platform.Students.Interfaces.Resources;
using ChildrenPath.Platform.Students.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Students.Interfaces.Rest;

[ApiController]
[Route("api/v1/students")]
[Route("api/v1/student-records")]
[Route("api/v1/studentRecords")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Students")]
public class StudentsController(
    IStudentCommandService commandService,
    IStudentQueryService queryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all students")]
    [ProducesResponseType(typeof(IEnumerable<StudentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var students = await queryService.Handle(new GetAllStudentsQuery(), cancellationToken);
        return Ok(students.Select(StudentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a student by ID")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var student = await queryService.Handle(new GetStudentByIdQuery(id), cancellationToken);
        return student == null
            ? NotFound(new { message = $"Student with id {id} not found." })
            : Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(student));
    }

    [HttpPost]
    [EndpointSummary("Register a student")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateStudentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateStudentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var student = await commandService.Handle(command, cancellationToken);
            var studentResource = StudentResourceFromEntityAssembler.ToResourceFromEntity(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, studentResource);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update a student")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateStudentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateStudentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var student = await commandService.Handle(command, cancellationToken);
            if (student == null) return NotFound(new { message = $"Student with id {id} not found." });

            return Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(student));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a student")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await commandService.Handle(new DeleteStudentCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound(new { message = $"Student with id {id} not found." });
    }
}