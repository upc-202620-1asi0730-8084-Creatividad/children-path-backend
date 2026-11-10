using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Queries;

namespace ChildrenPath.Platform.Alerts.Application.QueryServices;

/// <summary>
///     Represents the alert query service.
/// </summary>
public interface IAlertQueryService
{
    Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Alert>> Handle(GetAlertsByTripIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Alert>> Handle(GetAlertsByStatusQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Alert>> Handle(GetAllActiveAlertsQuery query, CancellationToken cancellationToken);
}