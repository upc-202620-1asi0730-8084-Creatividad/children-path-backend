using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Tracking.Application.CommandServices;
using ChildrenPath.Platform.Tracking.Application.QueryServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Commands;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;
using ChildrenPath.Platform.Tracking.Interfaces.Resources;
using ChildrenPath.Platform.Tracking.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;

namespace ChildrenPath.Platform.Tracking.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class TrackingController(
    ITrackingCommandService trackingCommandService,
    ITrackingQueryService trackingQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start tracking session")]
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
            return CreatedAtAction(nameof(StartTracking), new { id = sessionResource.Id }, sessionResource);
        });
    }

    [HttpGet("dashboard")]
    [EndpointSummary("Get active tracking dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var activeSessions = await trackingQueryService.Handle(new GetActiveTrackingSessionsQuery(), cancellationToken);
        
        var resources = activeSessions.Select(TrackingSessionResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        
        var response = new
        {
            summary = new
            {
                activeUnits = resources.Count,
                averageSpeed = resources.Any() ? resources.Average(r => r.CurrentSpeed) : 0,
                activeAlerts = 0
            },
            vehicles = resources
        };

        return Ok(response);
    }
}