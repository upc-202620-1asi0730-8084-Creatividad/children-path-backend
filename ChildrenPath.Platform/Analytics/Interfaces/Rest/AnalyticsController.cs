using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Analytics.Domain;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;

namespace ChildrenPath.Platform.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    // Mock de reportes guardados
    private static readonly List<Report> _mockReports = new()
    {
        new Report 
        { 
            Id = 1, Type = ReportType.Performance, Period = "Current week", GeneratedAt = DateTime.UtcNow.AddDays(-1),
            Metrics = new List<Metric> { new Metric { Name = "Total Trips", Value = 184, Unit = "trips" } },
            Kpis = new List<KPI> { new KPI { Code = "ONTIME", Description = "On-time rate", Target = 90, Actual = 91 } }
        },
        new Report 
        { 
            Id = 2, Type = ReportType.Attendance, Period = "Current month", GeneratedAt = DateTime.UtcNow.AddDays(-2),
            Metrics = new List<Metric> { new Metric { Name = "Transported Students", Value = 2450, Unit = "students" } }
        }
    };

    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        // Mapeo al formato que espera el frontend (analytics.store.js)
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
            reports = _mockReports.Select(r => new
            {
                id = $"r{r.Id}",
                type = r.Type.ToString().ToLower(),
                title = $"{r.Type} report",
                period = r.Period,
                generatedAt = r.GeneratedAt.ToString("o"),
                owner = "Company Admin",
                status = "ready",
                records = r.Metrics.FirstOrDefault()?.Value ?? 0
            }),
            activities = new[]
            {
                new { id = "a1", time = DateTime.UtcNow.AddHours(-1).ToString("o"), title = "Monitoring snapshot updated", description = "Live indicators were refreshed.", status = "healthy" }
            },
            lastUpdated = DateTime.UtcNow.ToString("o")
        };

        return Ok(response);
    }

    [HttpPost("reports")]
    public IActionResult CreateReport([FromBody] CreateReportResource resource)
    {
        var newId = _mockReports.Any() ? _mockReports.Max(r => r.Id) + 1 : 1;

        if (!Enum.TryParse<ReportType>(resource.Type, true, out var reportType))
            reportType = ReportType.Performance;

        var report = new Report
        {
            Id = newId,
            Type = reportType,
            Period = resource.Period,
            GeneratedAt = DateTime.UtcNow
        };

        // Simulamos la generación de métricas y KPIs
        report.Metrics.Add(new Metric { Name = "Generated Records", Value = 100, Unit = "records" });
        report.Kpis.Add(new KPI { Code = "QUALITY", Description = "Service Quality", Target = 90, Actual = 95 });
        report.Generate();

        _mockReports.Add(report);
        return CreatedAtAction(nameof(GetDashboard), new { id = report.Id }, report);
    }

    // Endpoint para exportar un reporte (del UML)
    [HttpGet("reports/{id:int}/export")]
    public IActionResult ExportReport(int id, [FromQuery] string format = "pdf")
    {
        var report = _mockReports.FirstOrDefault(r => r.Id == id);
        if (report == null) return NotFound();

        var result = report.Export(format);
        return Ok(new { message = result });
    }
}