using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Entities;

/// <summary>
///     Represents a reusable template for notifications.
/// </summary>
public class NotificationTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public string Content { get; set; } = string.Empty;

    public void UpdateContent(string newContent) => Content = newContent;
}