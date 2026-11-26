namespace ChildrenPath.Platform.Drivers.Domain.Model.ReadModels;

public record DriverReview(
    string Id,
    string DriverCode,
    string DriverName,
    string Title,
    string Description,
    DateTime? DueDate,
    string Priority);