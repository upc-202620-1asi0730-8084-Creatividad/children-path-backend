namespace ChildrenPath.Platform.Profiles.Interfaces.Resources;

public record CreateProfileResource(
    string FirstName,
    string LastName,
    string Phone,
    string Role, // "Parent", "Driver", "CompanyAdmin"

    string? LicenseNumber, 
    DateTime? LicenseExpiry,
    string? TaxId,
    string? LegalName,
    string? Relationship
);