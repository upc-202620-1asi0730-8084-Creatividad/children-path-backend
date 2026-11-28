using System.Net.Mime;
using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Application.QueryServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
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
    [EndpointSummary("get all vehicles")]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
    {
        var query = new GetAllVehiclesQuery();
        var vehicles = await vehicleQueryService.Handle(query, cancellationToken);
        return Ok(vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get vehicle by ID")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleById(int id, CancellationToken cancellationToken)
    {
        var query = new GetVehicleByIdQuery(id);
        var vehicle = await vehicleQueryService.Handle(query, cancellationToken);
        
        if (vehicle == null) return NotFound();

        return Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
    }

    [HttpGet("plate/{plate}")]
    [EndpointSummary("get vehicle by license plate")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleByPlate(string plate, CancellationToken cancellationToken)
    {
        var query = new GetVehicleByPlateQuery(plate);
        var vehicle = await vehicleQueryService.Handle(query, cancellationToken);
        
        if (vehicle == null) return NotFound();

        return Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
    }

    [HttpPost]
    [EndpointSummary("create a new vehicle")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            createdVehicle => 
            {
                var createdResource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(createdVehicle);
                return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.Id }, createdResource);
            });
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("update an existing vehicle")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updatedVehicle => Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(updatedVehicle)));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("delete a vehicle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVehicle(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteVehicleCommand(id);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }
}