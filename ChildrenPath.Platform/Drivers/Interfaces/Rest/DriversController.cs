using System.Net.Mime;
using ChildrenPath.Platform.Drivers.Application.CommandServices;
using ChildrenPath.Platform.Drivers.Application.QueryServices;
using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Drivers.Domain.Model.Queries;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;
using ChildrenPath.Platform.Drivers.Interfaces.Transform;
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
    [EndpointSummary("Get all drivers")]
    [ProducesResponseType(typeof(IEnumerable<DriverResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var drivers = await queryService.Handle(new GetAllDriversQuery(), cancellationToken);
        return Ok(drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a driver by ID")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var driver = await queryService.Handle(new GetDriverByIdQuery(id), cancellationToken);
        return driver == null
            ? NotFound(new { message = $"Driver with id {id} not found." })
            : Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(driver));
    }

    [HttpPost]
    [EndpointSummary("Register a driver")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDriverResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateDriverCommandFromResourceAssembler.ToCommandFromResource(resource);
            var driver = await commandService.Handle(command, cancellationToken);
            var driverResource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driverResource);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update a driver")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDriverResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var driver = await commandService.Handle(command, cancellationToken);
            return driver == null
                ? NotFound(new { message = $"Driver with id {id} not found." })
                : Ok(DriverResourceFromEntityAssembler.ToResourceFromEntity(driver));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a driver")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await commandService.Handle(new DeleteDriverCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound(new { message = $"Driver with id {id} not found." });
    }
}