using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Tracking.Domain;
using ChildrenPath.Platform.Tracking.Interfaces.Resources;

namespace ChildrenPath.Platform.Tracking.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class TrackingController : ControllerBase
{
    // Datos mock para simular el dashboard que espera Vue
    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        // Simulamos las sesiones activas (en un caso real, consultaríamos la BD)
        var sessions = new List<TrackingSession>
        {
            new TrackingSession 
            { 
                Id = 1, TripId = 101, VehicleId = 1, Status = TrackingStatus.Active, StartedAt = DateTime.UtcNow.AddHours(-1),
                Locations = new List<Location> { new Location { Latitude = -12.1195, Longitude = -77.0308, Speed = 31.4 } },
                ETAs = new List<ETA> { new ETA { MinutesRemaining = 7, EstimatedArrival = DateTime.UtcNow.AddMinutes(7), StopId = 1 } }
            },
            new TrackingSession 
            { 
                Id = 2, TripId = 102, VehicleId = 2, Status = TrackingStatus.Active, StartedAt = DateTime.UtcNow.AddMinutes(-45),
                Locations = new List<Location> { new Location { Latitude = -12.0968, Longitude = -77.0312, Speed = 18.6 } },
                ETAs = new List<ETA> { new ETA { MinutesRemaining = 12, EstimatedArrival = DateTime.UtcNow.AddMinutes(12), StopId = 2 } }
            }
        };

        // Mapeo al formato que espera el Frontend (trackingDashboard)
        var response = new
        {
            summary = new
            {
                activeUnits = sessions.Count,
                delayedUnits = 1,
                averageSpeed = 25.0,
                routeProgress = 76,
                gpsSignal = 98,
                activeAlerts = 3
            },
            vehicles = sessions.Select(s => new
            {
                id = $"trk-{s.VehicleId:D3}",
                vehicleId = s.VehicleId == 1 ? "KW-204" : "KW-118",
                plate = s.VehicleId == 1 ? "B7K-204" : "D4M-118",
                driverName = s.VehicleId == 1 ? "Carlos Pérez" : "María Gómez",
                routeName = s.VehicleId == 1 ? "Miraflores School Route" : "San Isidro Morning Route",
                schoolName = "Lima Norte School",
                district = s.VehicleId == 1 ? "Miraflores" : "San Isidro",
                status = "on-route",
                latitude = s.Locations.FirstOrDefault()?.Latitude ?? 0,
                longitude = s.Locations.FirstOrDefault()?.Longitude ?? 0,
                speedKmh = s.Locations.FirstOrDefault()?.Speed ?? 0,
                etaMinutes = s.ETAs.FirstOrDefault()?.MinutesRemaining ?? 0,
                progressPercent = 84,
                signalStrength = 99,
                studentCount = 16,
                nextStop = "Calle Pino 42",
                deviationMeters = 0,
                lastUpdate = "Today, 7:45 AM"
            }),
            alerts = new[]
            {
                new { id = "ta-001", type = "delay", title = "Route delay detected", description = "KW-118 is 8 minutes behind schedule near San Isidro.", severity = "high", vehicleId = "KW-118", time = "7:45 AM" }
            },
            activities = new[]
            {
                new { id = "tac-001", time = "7:45 AM", title = "Live position synchronized", description = "12 active units reported valid GPS coordinates.", status = "active" }
            }
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult StartTracking([FromBody] CreateTrackingResource resource)
    {
        var session = new TrackingSession
        {
            Id = new Random().Next(1, 1000),
            TripId = resource.TripId,
            VehicleId = resource.VehicleId,
            Status = TrackingStatus.Active
        };

        session.Locations.Add(new Location
        {
            Latitude = resource.InitialLatitude,
            Longitude = resource.InitialLongitude,
            Speed = resource.Speed
        });

        return CreatedAtAction(nameof(GetDashboard), new { id = session.Id }, session);
    }
}