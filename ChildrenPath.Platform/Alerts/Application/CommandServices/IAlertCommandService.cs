using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Alerts.Application.CommandServices;

/// <summary>
///     Represents the alert command service.
/// </summary>
public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(CancelAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(MarkAlertAsReadCommand command, CancellationToken cancellationToken);
}