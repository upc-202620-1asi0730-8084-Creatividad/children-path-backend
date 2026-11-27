namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a subscription plan by its numeric identifier.
/// </summary>
/// <param name="Id">The unique identifier of the plan.</param>
public record GetPlanByIdQuery(int Id);