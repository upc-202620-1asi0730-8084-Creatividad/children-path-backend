using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Application.QueryServices;

/// <summary>
///     Represents the query service contract for reading attendance records.
/// </summary>
public interface IAttendanceQueryService
{
    /// <summary>
    ///     Handles a query to retrieve all attendance records.
    /// </summary>
    /// <param name="query">The <see cref="GetAllAttendancesQuery" /> query.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection containing all attendance records.</returns>
    Task<IEnumerable<AttendanceEntity>> Handle(GetAllAttendancesQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles a query to retrieve a single attendance record by its identifier.
    /// </summary>
    /// <param name="query">The <see cref="GetAttendanceByIdQuery" /> query.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching <see cref="AttendanceEntity" /> if found; otherwise, <c>null</c>.</returns>
    Task<AttendanceEntity?> Handle(GetAttendanceByIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles a query to retrieve all attendance records for a specific trip.
    /// </summary>
    /// <param name="query">The <see cref="GetAttendancesByTripIdQuery" /> query.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of attendance records associated with the trip.</returns>
    Task<IEnumerable<AttendanceEntity>> Handle(GetAttendancesByTripIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles a query to retrieve all attendance records for a specific student.
    /// </summary>
    /// <param name="query">The <see cref="GetAttendancesByStudentIdQuery" /> query.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of attendance records associated with the student.</returns>
    Task<IEnumerable<AttendanceEntity>> Handle(GetAttendancesByStudentIdQuery query, CancellationToken cancellationToken);
}