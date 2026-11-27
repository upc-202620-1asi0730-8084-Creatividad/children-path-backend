namespace ChildrenPath.Platform.Profiles.Domain.Model.Entities;

public class DriverProfile
{
    public int Id { get; private set; }
    public string LicenseNumber { get; private set; }
    public DateTime LicenseExpiry { get; private set; }

    public DriverProfile() { LicenseNumber = string.Empty; }

    public DriverProfile(string licenseNumber, DateTime licenseExpiry)
    {
        LicenseNumber = licenseNumber;
        LicenseExpiry = licenseExpiry;
    }
}