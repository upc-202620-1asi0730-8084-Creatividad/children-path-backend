namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record UpdateDriverResource(
    string FullName,
    string Phone,
    string Email,
    string LicenseNumber,
    string LicenseClass,
    string? LicenseExpiresAt = null,
    string? AssignedVehicle = null,
    string? AssignedRoute = null,
    string? Status = null,
    int SafetyScore = 0,
    int PunctualityScore = 0,
    string? PhotoInitials = null,
    string? AvailabilityLabel = null,
    int TripsToday = 0,
    int StudentsAssigned = 0,
    DateTime? LastCheckIn = null,
    bool DocumentsValid = true,
    int YearsOfExperience = 0);