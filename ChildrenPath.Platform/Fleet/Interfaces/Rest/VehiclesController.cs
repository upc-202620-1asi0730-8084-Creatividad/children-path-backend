using System.Net.Mime;
using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Application.QueryServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vehicles")]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all vehicles")]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
    {
        var query = new GetAllVehiclesQuery();
        var vehicles = await vehicleQueryService.Handle(query, cancellationToken);
        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get vehicle by ID")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleById(int id, CancellationToken cancellationToken)
    {
        var query = new GetVehicleByIdQuery(id);
        var vehicle = await vehicleQueryService.Handle(query, cancellationToken);
        if (vehicle == null) return NotFound(new { message = $"Vehicle with id {id} not found." });

        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

    [HttpGet("plate/{plate}")]
    [EndpointSummary("Get vehicle by license plate")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleByPlate(string plate, CancellationToken cancellationToken)
    {
        var query = new GetVehicleByPlateQuery(plate);
        var vehicle = await vehicleQueryService.Handle(query, cancellationToken);
        if (vehicle == null) return NotFound(new { message = $"Vehicle with plate '{plate}' not found." });

        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

    [HttpPost]
    [EndpointSummary("Create a new vehicle")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
            var vehicle = await vehicleCommandService.Handle(command, cancellationToken);
            if (vehicle == null) return BadRequest(new { message = "Could not create vehicle." });

            var createdResource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, createdResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update an existing vehicle")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var updated = await vehicleCommandService.Handle(command, cancellationToken);
        if (updated == null) return NotFound(new { message = $"Vehicle with id {id} not found." });

        var updatedResource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(updated);
        return Ok(updatedResource);
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a vehicle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVehicle(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteVehicleCommand(id);
        var deleted = await vehicleCommandService.Handle(command, cancellationToken);
        if (!deleted) return NotFound(new { message = $"Vehicle with id {id} not found." });

        return NoContent();
    }
}
