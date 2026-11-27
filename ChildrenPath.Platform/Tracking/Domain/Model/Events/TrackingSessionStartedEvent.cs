using ChildrenPath.Platform.Shared.Domain.Model.Events;
using ChildrenPath.Platform.Tracking.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Tracking.Domain.Model.Events;

public record TrackingSessionStartedEvent(TrackingSessionId SessionId, int TripId, int VehicleId) : IEvent;