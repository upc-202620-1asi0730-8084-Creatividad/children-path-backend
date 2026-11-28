using ChildrenPath.Platform.Dashboard.Domain.Model.Queries;
using ChildrenPath.Platform.Dashboard.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Dashboard.Application.QueryServices;

public interface IDashboardQueryService
{
    Task<Result<DashboardSnapshot>> Handle(GetDashboardForRoleQuery query, CancellationToken cancellationToken);
}