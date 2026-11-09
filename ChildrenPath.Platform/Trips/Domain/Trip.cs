namespace ChildrenPath.Platform.Trips.Domain.Model.Aggregates;

public class Trip
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Shift { get; set; } = "morning";
    public string Status { get; set; } = "scheduled";
    public int Capacity { get; set; } = 18;
    public string StartTime { get; set; } = string.Empty;
    public string EstimatedEndTime { get; set; } = string.Empty;

    public Trip() {}
}