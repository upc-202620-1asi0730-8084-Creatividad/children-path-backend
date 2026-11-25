using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Tracking.Application.CommandServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Commands;
using ChildrenPath.Platform.Tracking.Interfaces.Resources;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;

namespace ChildrenPath.Platform.Tracking.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class TrackingController(ITrackingCommandService trackingCommandService) : ControllerBase
{
    // Endpoint limpio para crear una sesión de Tracking
    [HttpPost]
    public async Task<IActionResult> StartTracking([FromBody] CreateTrackingResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateTrackingSessionCommand(
            resource.TripId, 
            resource.VehicleId, 
            resource.InitialLatitude, 
            resource.InitialLongitude, 
            resource.Speed);

        var result = await trackingCommandService.Handle(command, cancellationToken);

        // Usamos el ResultActionResultAssembler del Shared
        return ResultActionResultAssembler.ToActionResult(this, result, session =>
        {
            // Mapeamos la entidad a un objeto anónimo (o Resource) para el frontend
            var response = new
            {
                id = session.Id.Value,
                tripId = session.TripId,
                vehicleId = session.VehicleId,
                status = session.Status.ToString(),
                startedAt = session.StartedAt,
                initialLocation = new
                {
                    lat = session.Locations.First().Latitude,
                    lng = session.Locations.First().Longitude,
                    speed = session.Locations.First().Speed
                }
            };

            return CreatedAtAction(nameof(StartTracking), new { id = session.Id.Value }, response);
        });
    }
}