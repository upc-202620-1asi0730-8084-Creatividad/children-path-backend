using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Dashboard.Interfaces.Resources;

namespace ChildrenPath.Platform.Dashboard.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController : ControllerBase
{
    [HttpGet]
    public IActionResult GetDashboard([FromQuery] string role = "company")
    {
        // Convertimos el string a Enum para usar la lógica de tu UML
        if (!Enum.TryParse<Domain.DashboardType>(role, true, out var dashboardType))
            dashboardType = Domain.DashboardType.Company;

        // Simulamos la carga de datos dependiendo del rol (Dashboard.Load())
        // En el futuro, aquí inyectaremos IVehicleRepository, IStudentRepository, etc.
        
        var response = dashboardType switch
        {
            Domain.DashboardType.Parent => BuildParentDashboard(),
            Domain.DashboardType.Driver => BuildDriverDashboard(),
            Domain.DashboardType.Administrator => BuildAdminDashboard(),
            _ => BuildCompanyDashboard() // Default
        };

        return Ok(new[] { response }); // El frontend espera un array y toma el [0]
    }

    private DashboardResponse BuildCompanyDashboard()
    {
        return new DashboardResponse(
            Role: "company",
            TitleKey: "dashboardPage.roles.company.title",
            SubtitleKey: "dashboardPage.roles.company.subtitle",
            HeroValue: "96.8%",
            HeroLabelKey: "dashboardPage.hero.company",
            Metrics: new List<object>
            {
                new { id = "trips", icon = "route", value = "12", labelKey = "dashboardPage.metrics.activeTrips", helperKey = "dashboardPage.helpers.activeTrips", trend = "+3", trendDirection = "up" },
                new { id = "students", icon = "school", value = "184", labelKey = "dashboardPage.metrics.studentsOnBoard", helperKey = "dashboardPage.helpers.students", trend = "+24", trendDirection = "up" },
                new { id = "alerts", icon = "notifications_active", value = "4", labelKey = "dashboardPage.metrics.openAlerts", helperKey = "dashboardPage.helpers.alerts", trend = "-2", trendDirection = "down" }
            },
            Vehicles: new List<object>
            {
                new { id = "kw-204", plate = "KW-204", driverName = "Carlos Pérez", lat = -12.0464, lng = -77.0428, status = "on-route", route = "Miraflores School Route", students = 18, etaMinutes = 7, lastUpdate = "2 min ago" },
                new { id = "kw-118", plate = "KW-118", driverName = "María Gómez", lat = -12.0931, lng = -77.0465, status = "delayed", route = "San Isidro Morning Route", students = 21, etaMinutes = 12, lastUpdate = "1 min ago" }
            },
            Alerts: new List<object>
            {
                new { id = 1, type = "delay", priority = "high", title = "Route delay detected", message = "Vehicle KW-118 is 8 minutes behind schedule.", timestamp = "Today, 7:45 AM", read = false, relatedVehicle = "KW-118" }
            },
            OperationItems: new List<object>
            {
                new { id = "attendance", label = "Attendance reliability", value = "96.8%", helper = "184 students confirmed", progress = 96.8, icon = "fact_check" }
            },
            Activities: new List<object>
            {
                new { id = "a1", time = "7:10 AM", title = "Morning dispatch completed", description = "12 routes started with assigned drivers.", status = "completed" }
            }
        );
    }

    // Puedes crear los otros métodos (BuildParentDashboard, etc.) con datos mock similares
    private DashboardResponse BuildParentDashboard() => new(
        Role: "parent", TitleKey: "dashboardPage.roles.operator.title", SubtitleKey: "dashboardPage.roles.operator.subtitle",
        HeroValue: "1", HeroLabelKey: "dashboardPage.hero.operator",
        Metrics: new List<object>(), Vehicles: new List<object>(), Alerts: new List<object>(), OperationItems: new List<object>(), Activities: new List<object>()
    );

    private DashboardResponse BuildDriverDashboard() => BuildParentDashboard();
    private DashboardResponse BuildAdminDashboard() => BuildCompanyDashboard();
}