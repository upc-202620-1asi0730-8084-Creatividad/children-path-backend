namespace ChildrenPath.Platform.Profiles.Interfaces.Resources;

public record CreateProfileResource(
    int UserId,
    string FirstName,
    string LastName,
    string Phone,
    string Role,

    string? LicenseNumber,
    DateTime? LicenseExpiry,
    string? TaxId,
    string? LegalName,
    string? Relationship
);