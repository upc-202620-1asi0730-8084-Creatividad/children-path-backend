namespace ChildrenPath.Platform.Drivers.Domain.Model.Entities;

public class DriverAvailability
{
    public int Id { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DriverId { get; set; }

    public bool CheckAvailability(DateTime date)
    {
        return date.DayOfWeek.ToString() == DayOfWeek;
    }
}
