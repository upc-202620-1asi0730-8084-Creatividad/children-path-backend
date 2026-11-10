using ChildrenPath.Platform.Alerts.Application.QueryServices;
using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Queries;
using ChildrenPath.Platform.Alerts.Domain.Repositories;

namespace ChildrenPath.Platform.Alerts.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken)
    {
        return await alertRepository.FindByIdAsync(query.AlertId, cancellationToken);
    }

    public async Task<IEnumerable<Alert>> Handle(GetAlertsByTripIdQuery query, CancellationToken cancellationToken)
    {
        return await alertRepository.FindByTripIdAsync(query.TripId, cancellationToken);
    }

    public async Task<IEnumerable<Alert>> Handle(GetAlertsByStatusQuery query, CancellationToken cancellationToken)
    {
        return await alertRepository.FindByStatusAsync(query.Status, cancellationToken);
    }

    public async Task<IEnumerable<Alert>> Handle(GetAllActiveAlertsQuery query, CancellationToken cancellationToken)
    {
        return await alertRepository.FindAllActiveAsync(cancellationToken);
    }
}