using ChildrenPath.Platform.Shared.Domain.Repositories;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Domain.Repositories;

/// <summary>
///     Represents the repository contract for managing <see cref="AttendanceEntity" /> aggregates.
/// </summary>
public interface IAttendanceRepository : IBaseRepository<AttendanceEntity>
{
    /// <summary>
    ///     Finds all attendance records associated with a specific trip.
    /// </summary>
    /// <param name="tripId">The unique identifier of the trip.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of attendance records belonging to the specified trip.</returns>
    Task<IEnumerable<AttendanceEntity>> FindByTripIdAsync(int tripId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds all attendance records associated with a specific student.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of attendance records belonging to the specified student.</returns>
    Task<IEnumerable<AttendanceEntity>> FindByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether an attendance record already exists for a given student on a specific trip.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="tripId">The unique identifier of the trip.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if a matching attendance record exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsByStudentIdAndTripIdAsync(int studentId, int tripId, CancellationToken cancellationToken = default);
}