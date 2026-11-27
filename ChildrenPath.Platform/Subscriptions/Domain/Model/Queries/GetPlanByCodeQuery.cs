namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a subscription plan by its unique code (e.g., starter, pro, enterprise).
/// </summary>
/// <param name="Code">The unique plan code.</param>
public record GetPlanByCodeQuery(string Code);