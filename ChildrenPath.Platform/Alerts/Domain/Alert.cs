namespace ChildrenPath.Platform.Alerts.Domain;

// --- ENUMS ---
public enum AlertType { Delay, RouteDeviation, SpeedViolation, Emergency, Absence }
public enum AlertPriority { Low, Medium, High, Critical }
public enum AlertStatus { Active, Resolved, Cancelled }
public enum NotificationChannel { Push, Email, SMS, WhatsApp }
public enum NotificationStatus { Pending, Sent, Delivered, Failed }

// --- ENTIDADES COMPUESTAS ---
public class DeliveryLog
{
    public int Id { get; set; }
    public string Response { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}

public class Notification
{
    public int Id { get; set; }
    public NotificationChannel Channel { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public int AlertId { get; set; } // FK

    // Composición
    public List<DeliveryLog> DeliveryLogs { get; set; } = new();

    public bool Send()
    {
        Status = NotificationStatus.Sent;
        return true;
    }

    public void MarkDelivered()
    {
        Status = NotificationStatus.Delivered;
    }
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class Alert
{
    public int Id { get; set; }
    public int TripId { get; set; } // Referencia al contexto de Trips
    public AlertType Type { get; set; }
    public AlertPriority Priority { get; set; }
    public string Message { get; set; } = string.Empty;
    public AlertStatus Status { get; set; } = AlertStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Composición: Una alerta dispara muchas notificaciones
    public List<Notification> Notifications { get; set; } = new();

    // Métodos del UML
    public void Activate() => Status = AlertStatus.Active;
    
    public bool Resolve()
    {
        Status = AlertStatus.Resolved;
        return true;
    }
}

// --- ENTIDADES DE CONFIGURACIÓN ---
public class NotificationTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public string Content { get; set; } = string.Empty;

    public void UpdateContent(string newContent) => Content = newContent;
}

public class AlertRule
{
    public int Id { get; set; }
    public AlertType Type { get; set; }
    public bool Enabled { get; set; } = true;
    public int Threshold { get; set; }

    public void Enable() => Enabled = true;
    public void Disable() => Enabled = false;
}