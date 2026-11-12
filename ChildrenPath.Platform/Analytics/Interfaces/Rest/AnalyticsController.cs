using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Analytics.Application.CommandServices;
using ChildrenPath.Platform.Analytics.Application.QueryServices;
using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Analytics.Domain.Model.Queries;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;
using ChildrenPath.Platform.Analytics.Interfaces.Transform;

namespace ChildrenPath.Platform.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService) 
    : ControllerBase
{
    [HttpGet("/api/v1/analyticsDashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var reports = await reportQueryService.Handle(new GetAnalyticsDashboardQuery(), cancellationToken);
        var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity).ToList();

        var response = new
        {
            summary = new
            {
                totalTrips = 184,
                transportedStudents = 2450,
                onTimeRate = 91,
                averageDelayMinutes = 7,
                attendanceRate = 96.8,
                serviceQualityScore = 93,
                fleetUsage = 88,
                incidentRate = 1.8
            },
            monitoringMetrics = new[]
            {
                new { id = "m1", label = "Active monitored routes", value = "14", helper = "Routes sending operational data", icon = "route", status = "healthy", trend = "up" },
                new { id = "m2", label = "Live tracking sessions", value = "12", helper = "Vehicles currently reporting GPS", icon = "gps_fixed", status = "healthy", trend = "stable" },
                new { id = "m3", label = "Attention signals", value = "3", helper = "Delays, incidents or attendance gaps", icon = "notification_important", status = "attention", trend = "down" }
            },
            routes = new[]
            {
                new { id = "rt-001", routeName = "Miraflores School Route", district = "Miraflores", driverName = "Carlos Pérez", vehiclePlate = "KW-204", onTimeRate = 96, attendanceRate = 98, delayMinutes = 4, completedTrips = 42, incidentCount = 0, serviceScore = 97, status = "healthy" },
                new { id = "rt-003", routeName = "Surco Pickup Route", district = "Santiago de Surco", driverName = "Luis Torres", vehiclePlate = "KW-076", onTimeRate = 78, attendanceRate = 92, delayMinutes = 18, completedTrips = 36, incidentCount = 2, serviceScore = 82, status = "risk" }
            },
            insights = new[]
            {
                new { id = "i1", title = "Surco route requires optimization", description = "Delay minutes increased during the last three service days.", impact = "high", source = "Route and trip analytics", action = "Review stop sequence" }
            },
            trend = new[]
            {
                new { label = "Mon", onTimeRate = 89, attendanceRate = 96, delayMinutes = 10 },
                new { label = "Tue", onTimeRate = 91, attendanceRate = 95, delayMinutes = 8 }
            },
            
            // 👇 AQUÍ USAMOS LOS DATOS REALES DE LA BD 👇
            reports = reportResources,
            
            activities = new[]
            {
                new { id = "a1", time = DateTime.UtcNow.AddHours(-1).ToString("o"), title = "Monitoring snapshot updated", description = "Live indicators were refreshed.", status = "healthy" }
            },
            lastUpdated = DateTime.UtcNow.ToString("o")
        };

        return Ok(response);
    }

    [HttpPost("reports")]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource resource, CancellationToken cancellationToken)
    {
        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reportCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Message);

        var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetDashboard), new { id = reportResource.Id }, reportResource);
    }

    [HttpGet("reports/{id:int}/export")]
    public async Task<IActionResult> ExportReport(int id, [FromQuery] string format = "pdf", CancellationToken cancellationToken = default)
    {
        var command = new ExportReportCommand(id, format);
        var result = await reportCommandService.Handle(command, cancellationToken);

        if (result.IsFailure) return NotFound(result.Message);

        return Ok(new { message = result.Value });
    }
}