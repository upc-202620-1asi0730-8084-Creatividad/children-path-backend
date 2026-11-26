namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record DriverResource(
    int Id,
    string Code,
    string FullName,
    string PhotoInitials,
    string LicenseNumber,
    string LicenseClass,
    DateTime? LicenseExpiresAt,
    string Phone,
    string Email,
    string AssignedVehicle,
    string AssignedRoute,
    string Status,
    string AvailabilityLabel,
    int TripsToday,
    int StudentsAssigned,
    int SafetyScore,
    int PunctualityScore,
    DateTime? LastCheckIn,
    bool DocumentsValid,
    int YearsOfExperience);