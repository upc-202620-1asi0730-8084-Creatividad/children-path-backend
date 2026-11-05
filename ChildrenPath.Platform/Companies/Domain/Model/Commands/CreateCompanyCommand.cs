namespace ChildrenPath.Platform.Companies.Domain.Model.Commands;

public record CreateCompanyCommand(
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
    string OperatingDistricts,
    bool Verified,
    DateTime? LicenseExpiration
);
