using ChildrenPath.Platform.Attendance.Application.QueryServices;
using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using ChildrenPath.Platform.Attendance.Domain.Repositories;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Application.Internal.QueryServices;

/// <summary>
///     Default implementation of <see cref="IAttendanceQueryService" />.
/// </summary>
/// <param name="attendanceRepository">The repository used to read attendance records.</param>
public class AttendanceQueryService(IAttendanceRepository attendanceRepository) : IAttendanceQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<AttendanceEntity>> Handle(
        GetAllAttendancesQuery query,
        CancellationToken cancellationToken)
    {
        return await attendanceRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AttendanceEntity?> Handle(
        GetAttendanceByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await attendanceRepository.FindByIdAsync(query.AttendanceId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttendanceEntity>> Handle(
        GetAttendancesByTripIdQuery query,
        CancellationToken cancellationToken)
    {
        return await attendanceRepository.FindByTripIdAsync(query.TripId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttendanceEntity>> Handle(
        GetAttendancesByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await attendanceRepository.FindByStudentIdAsync(query.StudentId, cancellationToken);
    }
}