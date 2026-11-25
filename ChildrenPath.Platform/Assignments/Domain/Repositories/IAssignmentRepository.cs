using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Assignments.Domain.Repositories;

/// <summary>
///     Repository contract for persisting and querying <see cref="Assignment" /> aggregates.
/// </summary>
public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    /// <summary>
    ///     Checks whether an assignment already exists for the specified student code and shift.
    /// </summary>
    /// <param name="studentCode">The student code to check.</param>
    /// <param name="shift">The service shift to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if a matching assignment exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsByStudentCodeAndShiftAsync(
        string studentCode,
        AssignmentShift shift,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether another assignment (excluding a given id) already exists for the specified student code and shift.
    /// </summary>
    /// <param name="studentCode">The student code to check.</param>
    /// <param name="shift">The service shift to check.</param>
    /// <param name="excludeId">The identifier of the current assignment to exclude from the check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if another matching assignment exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsByStudentCodeAndShiftExcludingIdAsync(
        string studentCode,
        AssignmentShift shift,
        int excludeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all assignments matching a specific lifecycle status.
    /// </summary>
    /// <param name="status">The target <see cref="AssignmentStatus" />.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of matching assignments.</returns>
    Task<IEnumerable<Assignment>> FindByStatusAsync(
        AssignmentStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all assignments scheduled for a specific shift.
    /// </summary>
    /// <param name="shift">The target <see cref="AssignmentShift" />.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of matching assignments.</returns>
    Task<IEnumerable<Assignment>> FindByShiftAsync(
        AssignmentShift shift,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all assignments belonging to a specific student code.
    /// </summary>
    /// <param name="studentCode">The student code to filter by.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of matching assignments.</returns>
    Task<IEnumerable<Assignment>> FindByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default);
}