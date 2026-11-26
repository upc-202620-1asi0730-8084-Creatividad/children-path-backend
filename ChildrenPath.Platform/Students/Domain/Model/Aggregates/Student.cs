using ChildrenPath.Platform.Students.Domain.Model.Commands;
using ChildrenPath.Platform.Students.Domain.Model.Entities;

namespace ChildrenPath.Platform.Students.Domain.Model.Aggregates;

public class Student
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;

    // Relaciones
    public List<Guardian> Guardians { get; set; } = new();
    public PickupPoint? PickupPoint { get; set; }
    public List<MedicalNote> MedicalNotes { get; set; } = new();

    // Extra para UI
    public string? RouteName { get; set; }
    public string? RouteCode { get; set; }
    public string? AssignedVehicle { get; set; }
    public string? AssignedDriver { get; set; }
    public string? DropOffPoint { get; set; }
    public string? PickupWindow { get; set; }
    public string AuthorizationStatus { get; set; } = "pending";
    public string LastAttendanceStatus { get; set; } = "pending";
    public string? EmergencyContact { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "unassigned";
    public int AttendanceRate { get; set; }

    public Student()
    {
    }

    public Student(CreateStudentCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        Grade = command.Grade;
        SchoolName = command.School;
        RouteName = command.RouteName;
        PickupWindow = command.PickupWindow;
        Status = command.Status;
        EmergencyContact = command.EmergencyContact;
        AuthorizationStatus = "pending";
        LastAttendanceStatus = "pending";

        if (!string.IsNullOrWhiteSpace(command.PickupPoint))
            PickupPoint = new PickupPoint { Address = command.PickupPoint };

        Guardians.Add(new Guardian
        {
            FullName = command.GuardianName,
            Phone = command.GuardianPhone,
            Email = command.GuardianEmail
        });
    }

    public void Update(UpdateStudentCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        Grade = command.Grade;
        SchoolName = command.School;
        Status = command.Status;
        EmergencyContact = command.EmergencyContact;
        RouteName = command.RouteName;
        PickupWindow = command.PickupWindow;

        var guardian = Guardians.OrderBy(item => item.Id).FirstOrDefault();
        if (guardian == null)
        {
            guardian = new Guardian();
            Guardians.Add(guardian);
        }

        guardian.FullName = command.GuardianName;
        guardian.Phone = command.GuardianPhone;
        guardian.Email = command.GuardianEmail;

        if (string.IsNullOrWhiteSpace(command.PickupPoint))
        {
            PickupPoint = null;
        }
        else if (PickupPoint == null)
        {
            PickupPoint = new PickupPoint { Address = command.PickupPoint };
        }
        else
        {
            PickupPoint.Address = command.PickupPoint;
        }
    }

    public void UpdateInfo(string firstName, string lastName, string grade)
    {
        FirstName = firstName;
        LastName = lastName;
        Grade = grade;
    }
}
