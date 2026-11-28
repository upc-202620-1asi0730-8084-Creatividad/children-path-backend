using ChildrenPath.Platform.Alerts.Domain.Model.Queries;
using ChildrenPath.Platform.Alerts.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Alerts.Application.QueryServices;

public interface IAlertDashboardQueryService
{
    Task<Result<AlertDashboardSnapshot>> Handle(
        GetAlertDashboardQuery query,
        CancellationToken cancellationToken);
}