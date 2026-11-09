using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Alerts.Domain;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;

namespace ChildrenPath.Platform.Alerts.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AlertsController : ControllerBase
{
    private static readonly List<Alert> _mockAlerts = new()
    {
        new Alert 
        { 
            Id = 1, TripId = 101, Type = AlertType.Delay, Priority = AlertPriority.High, 
            Message = "Vehicle KW-118 is 8 minutes behind schedule on San Isidro Morning Route.",
            Status = AlertStatus.Active, CreatedAt = DateTime.UtcNow.AddMinutes(-15),
            Notifications = new List<Notification>
            {
                new Notification { Id = 1, Channel = NotificationChannel.Push, Recipient = "Parent_123", Status = NotificationStatus.Delivered }
            }
        },
        new Alert 
        { 
            Id = 2, TripId = 102, Type = AlertType.Emergency, Priority = AlertPriority.Critical, 
            Message = "An incident report from KW-204 requires company admin review.",
            Status = AlertStatus.Active, CreatedAt = DateTime.UtcNow.AddHours(-1),
            Notifications = new List<Notification>()
        }
    };

    [HttpGet]
    public IActionResult GetDashboard()
    {
        // Mapeo al formato que espera el frontend (notifications.store.js)
        var items = _mockAlerts.Select(a => new
        {
            id = $"al-{a.Id:D3}",
            category = a.Type == AlertType.Delay || a.Type == AlertType.Emergency ? "alert" : "notification",
            type = a.Type.ToString().ToLower(),
            priority = a.Priority.ToString().ToLower(),
            status = "new", // Para el frontend
            title = a.Type == AlertType.Delay ? "Route delay detected" : "Minor incident follow-up",
            message = a.Message,
            time = a.CreatedAt.ToString("h:mm tt"),
            date = "Today",
            sourceBc = "Trip Management",
            recipientRole = "Company Admin",
            routeName = a.Type == AlertType.Delay ? "San Isidro Morning Route" : "Miraflores School Route",
            vehiclePlate = a.Type == AlertType.Delay ? "KW-118" : "KW-204",
            actionLabel = "Review alert"
        });

        var response = new
        {
            summary = new
            {
                total = items.Count(),
                unread = items.Count(),
                criticalAlerts = _mockAlerts.Count(a => a.Priority == AlertPriority.Critical),
                deliveryRate = 98.4,
                acknowledged = 0
            },
            items = items,
            activities = new[]
            {
                new { id = "act-001", time = "7:45 AM", title = "Delay alert generated", description = "KW-118 was marked as delayed.", status = "active" }
            }
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateAlert([FromBody] CreateAlertResource resource)
    {
        var newId = _mockAlerts.Any() ? _mockAlerts.Max(a => a.Id) + 1 : 1;

        // Parseo de Enums
        if (!Enum.TryParse<AlertType>(resource.Type, true, out var alertType))
            alertType = AlertType.Delay;
        if (!Enum.TryParse<AlertPriority>(resource.Priority, true, out var priority))
            priority = AlertPriority.Medium;
        if (!Enum.TryParse<NotificationChannel>(resource.Channel, true, out var channel))
            channel = NotificationChannel.Push;

        var alert = new Alert
        {
            Id = newId,
            TripId = resource.TripId,
            Type = alertType,
            Priority = priority,
            Message = resource.Message,
            Status = AlertStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        // Creamos la notificación asociada (Composición)
        var notification = new Notification
        {
            Id = newId,
            Channel = channel,
            Recipient = resource.Recipient,
            Status = NotificationStatus.Sent
        };
        
        // Simulamos el envío y registramos el log
        notification.Send();
        notification.DeliveryLogs.Add(new DeliveryLog { Response = "200 OK - Message dispatched", LoggedAt = DateTime.UtcNow });
        
        alert.Notifications.Add(notification);

        _mockAlerts.Add(alert);
        return CreatedAtAction(nameof(GetDashboard), new { id = alert.Id }, alert);
    }

    // Endpoint para resolver una alerta
    [HttpPatch("{id:int}/resolve")]
    public IActionResult Resolve(int id)
    {
        var alert = _mockAlerts.FirstOrDefault(a => a.Id == id);
        if (alert == null) return NotFound();

        alert.Resolve();
        return Ok(alert);
    }
}