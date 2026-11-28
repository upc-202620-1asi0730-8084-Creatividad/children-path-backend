using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Drivers.Application.CommandServices;

public interface IDriverCommandService
{
    Task<Result<Driver>> Handle(CreateDriverCommand command, CancellationToken cancellationToken = default);
    Task<Result<Driver>> Handle(UpdateDriverCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteDriverCommand command, CancellationToken cancellationToken = default);
}