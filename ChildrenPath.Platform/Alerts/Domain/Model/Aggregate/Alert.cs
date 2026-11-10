using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;

/// <summary>
///     Represents the Alert aggregate root within the Alerts Bounded Context.
/// </summary>
/// <remarks>
///     Encapsulates the core data and behavior of an alert, acting as an
///     aggregate root to ensure consistency of its contained entities.
/// </remarks>
public partial class Alert
{
    /// <summary>
    ///     Default constructor (used by persistence).
    /// </summary>
    public Alert()
    {
        Message = string.Empty;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Alert" /> aggregate.
    /// </summary>
    /// <param name="tripId">The trip identifier this alert belongs to.</param>
    /// <param name="type">The alert type.</param>
    /// <param name="priority">The alert priority.</param>
    /// <param name="message">The human-readable message.</param>
    public Alert(int tripId, AlertType type, AlertPriority priority, string message) : this()
    {
        TripId = tripId;
        Type = type;
        Priority = priority;
        Message = message;
    }

    /// <summary>Gets the unique identifier of the alert.</summary>
    public int Id { get; set; }

    /// <summary>Gets the trip identifier this alert belongs to.</summary>
    public int TripId { get; private set; }

    /// <summary>Gets the alert type.</summary>
    public AlertType Type { get; private set; }

    /// <summary>Gets the alert priority.</summary>
    public AlertPriority Priority { get; private set; }

    /// <summary>Gets the human-readable message.</summary>
    public string Message { get; private set; }

    /// <summary>Gets the creation timestamp.</summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}