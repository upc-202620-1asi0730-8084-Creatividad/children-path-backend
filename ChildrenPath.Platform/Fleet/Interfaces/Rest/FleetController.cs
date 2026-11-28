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
[Tags("Fleet")]
public class FleetController(
    IFleetCommandService fleetCommandService,
    IFleetQueryService fleetQueryService) : ControllerBase
{
    [HttpGet("dashboard")]
    [EndpointSummary("get fleet dashboard comprehensive data")]
    [ProducesResponseType(typeof(FleetDashboardResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFleetDashboard(CancellationToken cancellationToken)
    {
        var query = new GetFleetDashboardQuery();
        var data = await fleetQueryService.Handle(query, cancellationToken);
        var resource = FleetDashboardResourceAssembler.ToDashboardResource(data);
        return Ok(resource);
    }

    [HttpGet("summary")]
    [EndpointSummary("get fleet operational summary")]
    [ProducesResponseType(typeof(FleetSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFleetSummary(CancellationToken cancellationToken)
    {
        var query = new GetFleetSummaryQuery();
        var summary = await fleetQueryService.Handle(query, cancellationToken);
        var resource = FleetDashboardResourceAssembler.ToSummaryResource(summary);
        return Ok(resource);
    }

    [HttpGet("alerts")]
    [EndpointSummary("get all maintenance alerts")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceAlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMaintenanceAlerts(CancellationToken cancellationToken)
    {
        var query = new GetAllMaintenanceAlertsQuery();
        var alerts = await fleetQueryService.Handle(query, cancellationToken);
        var resources = alerts.Select(MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("alerts/{id:int}")]
    [EndpointSummary("get a maintenance alert by id")]
    [ProducesResponseType(typeof(MaintenanceAlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMaintenanceAlertById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetMaintenanceAlertByIdQuery(id);
        var alert = await fleetQueryService.Handle(query, cancellationToken);
        
        if (alert is null) return NotFound();

        return Ok(MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity(alert));
    }

    [HttpPost("alerts")]
    [EndpointSummary("create a maintenance alert")]
    [ProducesResponseType(typeof(MaintenanceAlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMaintenanceAlert(
        [FromBody] CreateMaintenanceAlertResource resource, 
        CancellationToken cancellationToken)
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

        var result = await fleetCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            createdAlert => 
            {
                var createdResource = MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity(createdAlert);
                return CreatedAtAction(
                    nameof(GetMaintenanceAlertById), 
                    new { id = createdAlert.Id }, 
                    createdResource);
            });
    }

    [HttpPatch("alerts/{id:int}/resolve")]
    [EndpointSummary("resolve a maintenance alert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveMaintenanceAlert(int id, CancellationToken cancellationToken)
    {
        var command = new ResolveMaintenanceAlertCommand(id);
        var result = await fleetCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => Ok(new { message = "Maintenance alert resolved successfully." }));
    }
}