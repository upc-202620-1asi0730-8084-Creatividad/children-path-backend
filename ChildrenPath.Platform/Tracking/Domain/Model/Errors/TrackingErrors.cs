using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Tracking.Domain.Model.Errors;

public static class TrackingErrors
{
    public static readonly Error SessionNotFound = Error.NotFound("Tracking.SessionNotFound", "Tracking session not found.");
    public static readonly Error InvalidCoordinates = Error.Validation("Tracking.InvalidCoordinates", "Latitude or longitude is invalid.");
    public static readonly Error SessionAlreadyStopped = Error.Conflict("Tracking.SessionAlreadyStopped", "The session is already stopped.");
}