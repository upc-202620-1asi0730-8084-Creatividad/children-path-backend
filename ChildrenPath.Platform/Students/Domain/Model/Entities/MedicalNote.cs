using ChildrenPath.Platform.Students.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Students.Domain.Model.Entities;

public class MedicalNote
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public MedicalSeverity Severity { get; set; } = MedicalSeverity.Low;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int StudentId { get; set; }
}
