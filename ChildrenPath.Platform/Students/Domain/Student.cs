namespace ChildrenPath.Platform.Students.Domain;

public enum MedicalSeverity { Low, Medium, High, Critical }

public class Guardian
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public int StudentId { get; set; }
}

public class PickupPoint
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int StudentId { get; set; }
}

public class MedicalNote
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public MedicalSeverity Severity { get; set; } = MedicalSeverity.Low;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int StudentId { get; set; }
}

public class Student
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // Para el frontend
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    
    // Relaciones
    public List<Guardian> Guardians { get; set; } = new();
    public PickupPoint? PickupPoint { get; set; }
    public List<MedicalNote> MedicalNotes { get; set; } = new();

    // Extra para tu UI
    public string? RouteName { get; set; }
    public string? AssignedVehicle { get; set; }
    public string Status { get; set; } = "unassigned";
    public int AttendanceRate { get; set; }

    public void UpdateInfo(string firstName, string lastName, string grade)
    {
        FirstName = firstName;
        LastName = lastName;
        Grade = grade;
    }
}