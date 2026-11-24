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
[Tags("Fleet")]
public class FleetController(
    IFleetCommandService fleetCommandService,
    IFleetQueryService fleetQueryService) : ControllerBase
{
    [HttpGet("dashboard")]
    [EndpointSummary("Get fleet dashboard comprehensive data")]
    [ProducesResponseType(typeof(FleetDashboardResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFleetDashboard(CancellationToken cancellationToken)
    {
        var query = new GetFleetDashboardQuery();
        var data = await fleetQueryService.Handle(query, cancellationToken);
        var resource = FleetDashboardResourceAssembler.ToDashboardResource(data);
        return Ok(resource);
    }

    [HttpGet("summary")]
    [EndpointSummary("Get fleet operational summary")]
    [ProducesResponseType(typeof(FleetSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFleetSummary(CancellationToken cancellationToken)
    {
        var query = new GetFleetSummaryQuery();
        var summary = await fleetQueryService.Handle(query, cancellationToken);
        var resource = FleetDashboardResourceAssembler.ToSummaryResource(summary);
        return Ok(resource);
    }

    [HttpGet("alerts")]
    [EndpointSummary("Get all maintenance alerts")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceAlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMaintenanceAlerts(CancellationToken cancellationToken)
    {
        var query = new GetAllMaintenanceAlertsQuery();
        var alerts = await fleetQueryService.Handle(query, cancellationToken);
        var resources = alerts.Select(MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("alerts")]
    [EndpointSummary("Create a maintenance alert")]
    [ProducesResponseType(typeof(MaintenanceAlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMaintenanceAlert([FromBody] CreateMaintenanceAlertResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateMaintenanceAlertCommand(
            resource.VehicleCode,
            resource.Plate,
            resource.Title,
            resource.DueDate,
            resource.Priority ?? "medium",
            resource.Description,
            resource.VehicleId
        );

        var alert = await fleetCommandService.Handle(command, cancellationToken);
        if (alert == null) return BadRequest(new { message = "Could not create maintenance alert." });

        var createdResource = MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
        return CreatedAtAction(nameof(GetAllMaintenanceAlerts), null, createdResource);
    }

    [HttpPatch("alerts/{id:int}/resolve")]
    [EndpointSummary("Resolve a maintenance alert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveMaintenanceAlert(int id, CancellationToken cancellationToken)
    {
        var command = new ResolveMaintenanceAlertCommand(id);
        var resolved = await fleetCommandService.Handle(command, cancellationToken);
        if (!resolved) return NotFound(new { message = $"Maintenance alert with id {id} not found." });

        return Ok(new { message = "Maintenance alert resolved successfully." });
    }
}
