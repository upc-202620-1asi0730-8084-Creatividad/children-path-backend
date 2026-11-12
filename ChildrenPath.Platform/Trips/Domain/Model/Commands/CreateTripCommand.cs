using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Commands;

/// <summary>
///     Command to create a new school transport trip.
/// </summary>
public record CreateTripCommand(
    string Code,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string School,
    string District,
    TripShift Shift,
    string StartTime,
    string EstimatedEndTime,
    int Capacity);