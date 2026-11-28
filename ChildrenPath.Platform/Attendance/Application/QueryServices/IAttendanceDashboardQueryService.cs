using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using ChildrenPath.Platform.Attendance.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Attendance.Application.QueryServices;

public interface IAttendanceDashboardQueryService
{
    Task<Result<AttendanceDashboardSnapshot>> Handle(
        GetAttendanceDashboardQuery query,
        CancellationToken cancellationToken);
}