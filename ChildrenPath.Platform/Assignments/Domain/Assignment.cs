namespace ChildrenPath.Platform.Assignments.Domain;

// --- ENUMS ---
public enum AssignmentStatus { Validated, Pending, Conflict, Inactive }
public enum AssignmentShift { Morning, Afternoon }
public enum AssignmentValidation { Ready, CapacityRisk, ScheduleConflict, MissingRoute, Inactive }

// --- ENTIDAD PRINCIPAL ---
public class Assignment
{
    public int Id { get; set; }
    
    // Datos del Estudiante
    public string StudentCode { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    
    // Datos de la Ruta y Vehículo
    public string RouteCode { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    
    // Datos de la Asignación
    public AssignmentShift Shift { get; set; } = AssignmentShift.Morning;
    public string PickupPoint { get; set; } = string.Empty;
    public string PickupWindow { get; set; } = string.Empty;
    
    // Validación y Estado
    public int CapacityUsage { get; set; }
    public int ValidationScore { get; set; }
    public AssignmentValidation Validation { get; set; } = AssignmentValidation.Ready;
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; } = string.Empty;

    // Métodos de Negocio (DDD)
    public void ValidateAssignment()
    {
        Status = AssignmentStatus.Validated;
        Validation = AssignmentValidation.Ready;
        ValidationScore = 100;
        LastUpdated = DateTime.UtcNow;
    }

    public void MarkAsConflict(string reason)
    {
        Status = AssignmentStatus.Conflict;
        Validation = AssignmentValidation.ScheduleConflict;
        Notes = reason;
        LastUpdated = DateTime.UtcNow;
    }
}