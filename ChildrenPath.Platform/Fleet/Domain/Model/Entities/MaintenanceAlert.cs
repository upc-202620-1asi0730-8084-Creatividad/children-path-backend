using ChildrenPath.Platform.Fleet.Domain.Model.Commands;

namespace ChildrenPath.Platform.Fleet.Domain.Model.Entities;

public class MaintenanceAlert
{
    public int Id { get; set; }
    public string VehicleCode { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public string Priority { get; set; } = "medium";
    public string Description { get; set; } = string.Empty;
    public int? VehicleId { get; set; }
    public bool IsResolved { get; set; }

    public MaintenanceAlert()
    {
    }

    public MaintenanceAlert(CreateMaintenanceAlertCommand command)
    {
        VehicleCode = command.VehicleCode;
        Plate = command.Plate;
        Title = command.Title;
        DueDate = command.DueDate;
        Priority = string.IsNullOrWhiteSpace(command.Priority) ? "medium" : command.Priority;
        Description = command.Description;
        VehicleId = command.VehicleId;
        IsResolved = false;
    }

    public void Resolve()
    {
        IsResolved = true;
    }
}
