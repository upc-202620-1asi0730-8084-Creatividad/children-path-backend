using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;

public class Driver
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string LicenseClass { get; set; } = string.Empty;
    public DateTime? LicenseExpiry { get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Available;
    public string PhotoInitials { get; set; } = string.Empty;
    public string AssignedVehicle { get; set; } = string.Empty;
    public string AssignedRoute { get; set; } = string.Empty;
    public string AvailabilityLabel { get; set; } = string.Empty;
    public int TripsToday { get; set; }
    public int StudentsAssigned { get; set; }
    public int SafetyScore { get; set; }
    public int PunctualityScore { get; set; }
    public DateTime? LastCheckIn { get; set; }
    public bool DocumentsValid { get; set; } = true;
    public int YearsOfExperience { get; set; }

    public List<DriverAvailability> Availabilities { get; set; } = new();
    public List<DriverAssignment> Assignments { get; set; } = new();

    public Driver()
    {
    }

    public Driver(CreateDriverCommand command)
    {
        Apply(command.FullName, command.Phone, command.Email, command.LicenseNumber, command.LicenseClass,
            command.LicenseExpiresAt, command.AssignedVehicle, command.AssignedRoute, command.Status,
            command.SafetyScore, command.PunctualityScore, command.PhotoInitials, command.AvailabilityLabel,
            command.TripsToday, command.StudentsAssigned, command.LastCheckIn, command.DocumentsValid,
            command.YearsOfExperience);
    }

    public void Update(UpdateDriverCommand command)
    {
        Apply(command.FullName, command.Phone, command.Email, command.LicenseNumber, command.LicenseClass,
            command.LicenseExpiresAt, command.AssignedVehicle, command.AssignedRoute, command.Status,
            command.SafetyScore, command.PunctualityScore, command.PhotoInitials, command.AvailabilityLabel,
            command.TripsToday, command.StudentsAssigned, command.LastCheckIn, command.DocumentsValid,
            command.YearsOfExperience);
    }

    public void UpdateAvailability(DriverStatus newStatus) => Status = newStatus;

    private void Apply(
        string fullName,
        string phone,
        string email,
        string licenseNumber,
        string licenseClass,
        DateTime? licenseExpiresAt,
        string? assignedVehicle,
        string? assignedRoute,
        DriverStatus status,
        int safetyScore,
        int punctualityScore,
        string? photoInitials,
        string? availabilityLabel,
        int tripsToday,
        int studentsAssigned,
        DateTime? lastCheckIn,
        bool documentsValid,
        int yearsOfExperience)
    {
        FullName = fullName;
        Phone = phone;
        Email = email;
        LicenseNumber = licenseNumber;
        LicenseClass = licenseClass;
        LicenseExpiry = licenseExpiresAt;
        AssignedVehicle = assignedVehicle ?? string.Empty;
        AssignedRoute = assignedRoute ?? string.Empty;
        Status = status;
        SafetyScore = safetyScore;
        PunctualityScore = punctualityScore;
        PhotoInitials = photoInitials ?? string.Empty;
        AvailabilityLabel = availabilityLabel ?? string.Empty;
        TripsToday = tripsToday;
        StudentsAssigned = studentsAssigned;
        LastCheckIn = lastCheckIn;
        DocumentsValid = documentsValid;
        YearsOfExperience = yearsOfExperience;
    }
}
