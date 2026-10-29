namespace ChildrenPath.Platform.Fleet.Domain;

public class Vehicle
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "available";
    public string DriverName { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public int AssignedStudents { get; set; }
    
    public Vehicle() {}
}