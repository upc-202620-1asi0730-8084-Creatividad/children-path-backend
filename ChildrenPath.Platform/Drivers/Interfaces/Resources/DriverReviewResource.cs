namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record DriverReviewResource(
    string Id,
    string DriverCode,
    string DriverName,
    string Title,
    string Description,
    DateTime? DueDate,
    string Priority);