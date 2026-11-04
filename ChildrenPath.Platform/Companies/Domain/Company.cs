namespace ChildrenPath.Platform.Companies.Domain;

public class Company
{
    public int Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string CommercialName { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public string Status { get; set; } = "active"; // active, review, pending, suspended
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
}