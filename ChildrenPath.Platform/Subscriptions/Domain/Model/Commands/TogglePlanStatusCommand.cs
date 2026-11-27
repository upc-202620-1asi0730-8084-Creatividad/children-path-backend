namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to activate or deactivate a subscription plan.
/// </summary>
/// <param name="PlanId">The unique identifier of the plan.</param>
/// <param name="IsActive"><c>true</c> to activate the plan; <c>false</c> to deactivate it.</param>
public record TogglePlanStatusCommand(int PlanId, bool IsActive);