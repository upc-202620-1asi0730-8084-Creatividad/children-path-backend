using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

namespace ChildrenPath.Platform.Subscriptions.Application.CommandServices;

/// <summary>
///     Application command service contract for mutating <see cref="Plan" /> aggregates.
/// </summary>
public interface IPlanCommandService
{
    Task<Result<Plan>> Handle(CreatePlanCommand command, CancellationToken cancellationToken = default);
    Task<Result<Plan>> Handle(UpdatePlanCommand command, CancellationToken cancellationToken = default);
    Task<Result<Plan>> Handle(TogglePlanStatusCommand command, CancellationToken cancellationToken = default);
}