namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record CreateDriverResource(
    string FullName,
    string Phone,
    string Email,
    string LicenseNumber,
    DateTime LicenseExpiry,
    int SafetyScore,
    int PunctualityScore,
    string Status
);