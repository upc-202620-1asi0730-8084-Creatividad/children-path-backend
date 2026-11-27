namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all subscriptions belonging to a specific user.
/// </summary>
/// <param name="UserId">The identifier of the user.</param>
public record GetSubscriptionsByUserIdQuery(int UserId);