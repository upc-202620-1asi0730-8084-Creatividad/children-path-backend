using System.Net.Mime;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
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
[Produces(MediaTypeNames.Application.Json)]
[Tags("Students")]
public class StudentsController(
    IStudentCommandService commandService,
    IStudentQueryService queryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("get all students")]
    [ProducesResponseType(typeof(IEnumerable<StudentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var students = await queryService.Handle(new GetAllStudentsQuery(), cancellationToken);
        return Ok(students.Select(StudentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get a student by ID")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var student = await queryService.Handle(new GetStudentByIdQuery(id), cancellationToken);
        if (student == null) return NotFound();

        return Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(student));
    }

    [HttpPost]
    [EndpointSummary("register a student")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentResource resource, CancellationToken cancellationToken)
    {
        var command = CreateStudentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => 
            {
                var resource = StudentResourceFromEntityAssembler.ToResourceFromEntity(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, resource);
            });
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("update a student")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateStudentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("delete a student")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new DeleteStudentCommand(id), cancellationToken);
        
        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }
}