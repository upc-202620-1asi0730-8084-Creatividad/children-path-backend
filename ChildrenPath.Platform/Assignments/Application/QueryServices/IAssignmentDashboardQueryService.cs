using ChildrenPath.Platform.Assignments.Domain.Model.Queries;
using ChildrenPath.Platform.Assignments.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Assignments.Application.QueryServices;

public interface IAssignmentDashboardQueryService
{
    Task<Result<AssignmentDashboardSnapshot>> Handle(
        GetAssignmentDashboardQuery query,
        CancellationToken cancellationToken);
}