using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Assignments.Application.CommandServices;

/// <summary>
///     Application command service contract for mutating <see cref="Assignment" /> aggregates.
/// </summary>
public interface IAssignmentCommandService
{
    Task<Result<Assignment>> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Assignment>> Handle(UpdateAssignmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Assignment>> Handle(ValidateAssignmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Assignment>> Handle(MarkAssignmentConflictCommand command, CancellationToken cancellationToken = default);
    Task<Result<bool>> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken = default);
}