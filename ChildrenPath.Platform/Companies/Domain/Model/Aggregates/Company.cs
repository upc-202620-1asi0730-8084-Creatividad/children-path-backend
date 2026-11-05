using ChildrenPath.Platform.Companies.Domain.Model.Commands;

namespace ChildrenPath.Platform.Companies.Domain.Model.Aggregates;

public class Company
{
    public int Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string CommercialName { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public string Plan { get; set; } = string.Empty;
    public int PlanLimitVehicles { get; set; }
    public int PlanLimitStudents { get; set; }
    public string AdminName { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string OperatingDistricts { get; set; } = string.Empty;
    public bool Verified { get; set; }
    public DateTime? LicenseExpiration { get; set; }

    public Company()
    {
    }

    public Company(CreateCompanyCommand command)
    {
        LegalName = command.LegalName;
        CommercialName = command.CommercialName;
        Ruc = command.Ruc;
        Status = string.IsNullOrWhiteSpace(command.Status) ? "active" : command.Status;
        Plan = command.Plan;
        PlanLimitVehicles = command.PlanLimitVehicles;
        PlanLimitStudents = command.PlanLimitStudents;
        AdminName = command.AdminName;
        AdminEmail = command.AdminEmail;
        Phone = command.Phone;
        Address = command.Address;
        OperatingDistricts = command.OperatingDistricts;
        Verified = command.Verified;
        LicenseExpiration = command.LicenseExpiration;
    }

    public void Update(UpdateCompanyCommand command)
    {
        LegalName = command.LegalName;
        CommercialName = command.CommercialName;
        Ruc = command.Ruc;
        Status = command.Status;
        Plan = command.Plan;
        PlanLimitVehicles = command.PlanLimitVehicles;
        PlanLimitStudents = command.PlanLimitStudents;
        AdminName = command.AdminName;
        AdminEmail = command.AdminEmail;
        Phone = command.Phone;
        Address = command.Address;
        OperatingDistricts = command.OperatingDistricts;
        Verified = command.Verified;
        LicenseExpiration = command.LicenseExpiration;
    }
}
