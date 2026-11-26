using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.Commands;

namespace ChildrenPath.Platform.Drivers.Application.CommandServices;

public interface IDriverCommandService
{
    Task<Driver> Handle(CreateDriverCommand command, CancellationToken cancellationToken = default);
    Task<Driver?> Handle(UpdateDriverCommand command, CancellationToken cancellationToken = default);
    Task<bool> Handle(DeleteDriverCommand command, CancellationToken cancellationToken = default);
}