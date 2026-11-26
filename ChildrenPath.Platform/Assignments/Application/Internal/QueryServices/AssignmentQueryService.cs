using ChildrenPath.Platform.Assignments.Application.QueryServices;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.Queries;
using ChildrenPath.Platform.Assignments.Domain.Repositories;

namespace ChildrenPath.Platform.Assignments.Application.Internal.QueryServices;

/// <summary>
///     Default implementation of <see cref="IAssignmentQueryService" />.
/// </summary>
/// <param name="assignmentRepository">The repository used to query assignments.</param>
public class AssignmentQueryService(IAssignmentRepository assignmentRepository) : IAssignmentQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> Handle(
        GetAllAssignmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await assignmentRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Assignment?> Handle(
        GetAssignmentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await assignmentRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> Handle(
        GetAssignmentsByStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        return await assignmentRepository.FindByStatusAsync(query.Status, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> Handle(
        GetAssignmentsByShiftQuery query,
        CancellationToken cancellationToken = default)
    {
        return await assignmentRepository.FindByShiftAsync(query.Shift, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> Handle(
        GetAssignmentsByStudentCodeQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.StudentCode))
            return [];

        return await assignmentRepository.FindByStudentCodeAsync(query.StudentCode, cancellationToken);
    }
}