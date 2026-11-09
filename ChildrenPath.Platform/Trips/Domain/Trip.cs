namespace ChildrenPath.Platform.Trips.Domain;

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
    public int Students { get; set; } = 0;
    public string StartTime { get; set; } = string.Empty;
    public string EstimatedEndTime { get; set; } = string.Empty;

    public void Complete() => Status = "completed";
}