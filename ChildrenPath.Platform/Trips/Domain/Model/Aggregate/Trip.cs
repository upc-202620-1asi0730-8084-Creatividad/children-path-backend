using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Aggregate;

/// <summary>
///     Represents the Trip aggregate root within the Trips Bounded Context.
/// </summary>
public partial class Trip
{
    /// <summary>
    ///     Default constructor (used by persistence).
    /// </summary>
    public Trip()
    {
        Code = string.Empty;
        RouteName = string.Empty;
        VehiclePlate = string.Empty;
        DriverName = string.Empty;
        School = string.Empty;
        District = string.Empty;
        StartTime = string.Empty;
        EstimatedEndTime = string.Empty;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Trip" /> aggregate.
    /// </summary>
    public Trip(
        string code,
        string routeName,
        string vehiclePlate,
        string driverName,
        string school,
        string district,
        TripShift shift,
        string startTime,
        string estimatedEndTime,
        int capacity) : this()
    {
        Code = code;
        RouteName = routeName;
        VehiclePlate = vehiclePlate;
        DriverName = driverName;
        School = school;
        District = district;
        Shift = shift;
        StartTime = startTime;
        EstimatedEndTime = estimatedEndTime;
        Capacity = capacity;
    }

    public int Id { get; set; }
    public string Code { get; private set; }
    public string RouteName { get; private set; }
    public string VehiclePlate { get; private set; }
    public string DriverName { get; private set; }
    public string School { get; private set; }
    public string District { get; private set; }
    public TripShift Shift { get; private set; } = TripShift.Morning;
    public string StartTime { get; private set; }
    public string EstimatedEndTime { get; private set; }
    public int Capacity { get; private set; } = 18;
    public int Students { get; private set; } = 0;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}