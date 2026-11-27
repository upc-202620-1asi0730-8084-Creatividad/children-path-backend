namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve the active subscription belonging to a specific user.
/// </summary>
/// <param name="UserId">The identifier of the user.</param>
public record GetActiveSubscriptionByUserIdQuery(int UserId);