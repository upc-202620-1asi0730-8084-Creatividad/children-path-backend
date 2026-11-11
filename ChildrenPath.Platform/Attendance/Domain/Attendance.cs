namespace ChildrenPath.Platform.Attendance.Domain;

// --- ENUM ---
public enum AttendanceStatus { Boarded, DroppedOff, Absent, Late }

// --- ENTIDADES DETALLE ---
public class BoardingRecord
{
    public int Id { get; set; }
    
    public int AttendanceId { get; set; } 
    public DateTime BoardedAt { get; set; } = DateTime.UtcNow;
    public string Location { get; set; } = string.Empty;

    public void Record(string location)
    {
        Location = location;
        BoardedAt = DateTime.UtcNow;
    }
}

public class AbsenceRecord
{
    public int Id { get; set; }
    
    public int AttendanceId { get; set; } 
    public string Reason { get; set; } = string.Empty;
    public bool Justified { get; set; }

    public void Register(string reason, bool justified)
    {
        Reason = reason;
        Justified = justified;
    }
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class Attendance
{
    public int Id { get; set; }
    
    // Referencias a otros contextos (como indica tu UML)
    public int StudentId { get; set; }
    public int TripId { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Boarded;

    // Composición (según tu UML)
    public BoardingRecord? BoardingRecord { get; set; }
    public AbsenceRecord? AbsenceRecord { get; set; }

    // --- Métodos de Negocio (del UML) ---
    public void MarkBoarding(string location)
    {
        Status = AttendanceStatus.Boarded;
        BoardingRecord = new BoardingRecord();
        BoardingRecord.Record(location);
        Timestamp = DateTime.UtcNow;
    }

    public void MarkDropOff()
    {
        Status = AttendanceStatus.DroppedOff;
        Timestamp = DateTime.UtcNow;
    }

    public void MarkAbsent(string reason, bool justified)
    {
        Status = AttendanceStatus.Absent;
        AbsenceRecord = new AbsenceRecord();
        AbsenceRecord.Register(reason, justified);
        Timestamp = DateTime.UtcNow;
    }
}