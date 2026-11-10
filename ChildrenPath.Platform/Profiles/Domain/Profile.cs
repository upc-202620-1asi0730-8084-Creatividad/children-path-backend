namespace ChildrenPath.Platform.Profiles.Domain;

// 1. Enum de Roles
public enum UserRole
{
    Parent,
    Driver,
    CompanyAdmin,
    ChildrenPathAdmin
}

// 2. Entidades Compuestas
public class Preferences
{
    public int Id { get; set; }
    public string Language { get; set; } = "en";
    public string Timezone { get; set; } = "America/Lima";
    public bool NotificationsEnabled { get; set; } = true;
}

public class ParentProfile
{
    public int Id { get; set; }
    public string Relationship { get; set; } = string.Empty;
    public int ChildrenCount { get; set; }
    
    public bool LinkStudent(int studentId) => true; // Lógica simple
}

public class DriverProfile
{
    public int Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime LicenseExpiry { get; set; }
}

public class CompanyProfile
{
    public int Id { get; set; }
    public string TaxId { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
}

// 3. Entidad Principal (Aggregate Root)
public class Profile
{
    public int Id { get; set; }
    public int UserId { get; set; } // FK hacia IAM (para el futuro)
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    
    public UserRole Role { get; set; } = UserRole.Parent;

    // Relaciones de Composición (Uno a Uno)
    public Preferences Preferences { get; set; } = new();
    public ParentProfile? ParentProfile { get; set; }
    public DriverProfile? DriverProfile { get; set; }
    public CompanyProfile? CompanyProfile { get; set; }

    // Método del diagrama
    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}