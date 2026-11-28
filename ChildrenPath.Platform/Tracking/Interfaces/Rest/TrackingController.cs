using System.Net.Mime;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Tracking.Application.CommandServices;
using ChildrenPath.Platform.Tracking.Application.QueryServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Commands;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;
using ChildrenPath.Platform.Tracking.Interfaces.Resources;
using ChildrenPath.Platform.Tracking.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Tracking.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Tracking")]
public class TrackingController(
    ITrackingCommandService trackingCommandService,
    ITrackingQueryService trackingQueryService,
    ITrackingDashboardQueryService trackingDashboardQueryService)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start a tracking session")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrackingSessionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartTracking([FromBody] CreateTrackingResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateTrackingSessionCommand(
            resource.TripId, 
            resource.VehicleId, 
            resource.InitialLatitude, 
            resource.InitialLongitude, 
            resource.Speed);

        var result = await trackingCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, session =>
        {
            var sessionResource = TrackingSessionResourceFromEntityAssembler.ToResourceFromEntity(session);
            return CreatedAtAction(nameof(GetSessionById), new { id = sessionResource.Id }, sessionResource);
        });
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a tracking session by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackingSessionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var session = await trackingQueryService.Handle(new GetTrackingSessionByIdQuery(id), cancellationToken);
        if (session is null) return NotFound();
        
        return Ok(TrackingSessionResourceFromEntityAssembler.ToResourceFromEntity(session));
    }

    [HttpGet("dashboard")]
    [EndpointSummary("Get active tracking dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackingDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await trackingDashboardQueryService.Handle(new GetTrackingDashboardQuery(), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            snapshot => 
            {
                var vehicles = snapshot.Sessions.Select(TrackingSessionResourceFromEntityAssembler.ToResourceFromEntity);
                
                var dashboardResource = new TrackingDashboardResource(
                    Summary: new TrackingDashboardSummaryResource(
                        snapshot.Summary.ActiveUnits,
                        snapshot.Summary.AverageSpeed,
                        snapshot.Summary.ActiveAlerts),
                    Vehicles: vehicles
                );
                
                return Ok(dashboardResource);
            });
    }
}