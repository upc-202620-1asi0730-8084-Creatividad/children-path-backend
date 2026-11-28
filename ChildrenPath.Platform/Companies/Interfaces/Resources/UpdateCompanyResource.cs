namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record UpdateCompanyResource(
    string LegalName,
    string CommercialName,
    string Ruc,
    string Status,
    string Plan,
    int PlanLimitVehicles,
    int PlanLimitStudents,
    string AdminName,
    string AdminEmail,
    string Phone,
    string Address,
    IEnumerable<string>? OperatingDistricts,
    bool Verified,
    DateTime? LicenseExpiration
);
