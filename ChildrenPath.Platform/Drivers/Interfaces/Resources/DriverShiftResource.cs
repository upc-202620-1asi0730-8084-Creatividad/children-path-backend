namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record DriverShiftResource(
    int Id,
    string Time,
    string Title,
    string Description,
    string DriverName,
    string Status);