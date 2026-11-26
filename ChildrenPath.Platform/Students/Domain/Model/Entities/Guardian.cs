namespace ChildrenPath.Platform.Students.Domain.Model.Entities;

public class Guardian
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Relationship { get; set; } = string.Empty;
    public int StudentId { get; set; }
}
