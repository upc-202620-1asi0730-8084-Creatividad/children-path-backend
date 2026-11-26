using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Drivers.Domain.Model.Commands;

public record UpdateDriverCommand(
    int Id,
    string FullName,
    string Phone,
    string Email,
    string LicenseNumber,
    string LicenseClass,
    DateTime? LicenseExpiresAt,
    string? AssignedVehicle,
    string? AssignedRoute,
    DriverStatus Status,
    int SafetyScore,
    int PunctualityScore,
    string? PhotoInitials,
    string? AvailabilityLabel,
    int TripsToday,
    int StudentsAssigned,
    DateTime? LastCheckIn,
    bool DocumentsValid,
    int YearsOfExperience);