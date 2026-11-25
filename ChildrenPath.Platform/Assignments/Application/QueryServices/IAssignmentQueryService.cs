using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.Queries;

namespace ChildrenPath.Platform.Assignments.Application.QueryServices;

/// <summary>
///     Application query service contract for reading <see cref="Assignment" /> aggregates.
/// </summary>
public interface IAssignmentQueryService
{
    Task<IEnumerable<Assignment>> Handle(GetAllAssignmentsQuery query, CancellationToken cancellationToken = default);
    Task<Assignment?> Handle(GetAssignmentByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Assignment>> Handle(GetAssignmentsByStatusQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Assignment>> Handle(GetAssignmentsByShiftQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Assignment>> Handle(GetAssignmentsByStudentCodeQuery query, CancellationToken cancellationToken = default);
}