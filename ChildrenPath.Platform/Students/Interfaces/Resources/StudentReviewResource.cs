namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record StudentReviewResource(
    string Id,
    string StudentCode,
    string StudentName,
    string Title,
    string Description,
    string DueDate,
    string Priority);