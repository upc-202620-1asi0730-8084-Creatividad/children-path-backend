namespace ChildrenPath.Platform.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
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