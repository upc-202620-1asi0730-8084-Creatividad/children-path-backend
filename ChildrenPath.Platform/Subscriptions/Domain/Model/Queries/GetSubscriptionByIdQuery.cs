namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a single subscription by its identifier.
/// </summary>
/// <param name="Id">The unique identifier of the subscription.</param>
public record GetSubscriptionByIdQuery(int Id);