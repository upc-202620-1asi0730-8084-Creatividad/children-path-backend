namespace ChildrenPath.Platform.Students.Domain.Model.ReadModels;

public record StudentReview(
    string Id,
    string StudentCode,
    string StudentName,
    string Title,
    string Description,
    DateTime? DueDate,
    string Priority);