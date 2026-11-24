using ChildrenPath.Platform.Fleet.Domain.Model.Commands;

namespace ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;

public class TransportFleet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public TransportFleet()
    {
    }

    public TransportFleet(CreateFleetCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        CompanyId = command.CompanyId;
    }
}
