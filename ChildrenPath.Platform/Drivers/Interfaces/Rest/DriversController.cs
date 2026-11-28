using System.Net.Mime;
using ChildrenPath.Platform.Drivers.Application.CommandServices;
using ChildrenPath.Platform.Drivers.Application.QueryServices;
using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Drivers.Domain.Model.Queries;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;
using ChildrenPath.Platform.Drivers.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Drivers.Interfaces.Rest;

[ApiController]
[Route("api/v1/drivers")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Drivers")]
public class DriversController(
    IDriverCommandService commandService,
    IDriverQueryService queryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("get all drivers")]
    [ProducesResponseType(typeof(IEnumerable<DriverResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var drivers = await queryService.Handle(new GetAllDriversQuery(), cancellationToken);
        return Ok(drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get a driver by ID")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var driver = await queryService.Handle(new GetDriverByIdQuery(id), cancellationToken);
        if (driver == null) return NotFound();

        return Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(driver));
    }

    [HttpPost]
    [EndpointSummary("register a driver")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDriverResource resource, CancellationToken cancellationToken)
    {
        var command = CreateDriverCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this, 
            result, 
            created => 
            {
                var driverResource = DriverResourceFromEntityAssembler.ToResourceFromEntity(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, driverResource);
            });
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("update a driver")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDriverResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this, 
            result, 
            updated => Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("delete a driver")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new DeleteDriverCommand(id), cancellationToken);
        
        return ResultActionResultAssembler.ToActionResult(
            this, 
            result, 
            _ => NoContent());
    }
}