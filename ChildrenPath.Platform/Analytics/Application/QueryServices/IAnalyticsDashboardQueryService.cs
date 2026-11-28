using ChildrenPath.Platform.Analytics.Domain.Model.Queries;
using ChildrenPath.Platform.Analytics.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Analytics.Application.QueryServices;

public interface IAnalyticsDashboardQueryService
{
    Task<Result<AnalyticsDashboardSnapshot>> Handle(
        GetAnalyticsDashboardQuery query,
        CancellationToken cancellationToken);
}