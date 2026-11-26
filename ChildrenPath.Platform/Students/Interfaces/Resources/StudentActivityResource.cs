namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record StudentActivityResource(
    string Id,
    string Time,
    string Title,
    string Description,
    string StudentName,
    string Status);