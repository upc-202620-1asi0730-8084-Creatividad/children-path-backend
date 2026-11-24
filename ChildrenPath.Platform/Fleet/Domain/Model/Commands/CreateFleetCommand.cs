namespace ChildrenPath.Platform.Fleet.Domain.Model.Commands;

public record CreateFleetCommand(string Name, string Description, int CompanyId);
