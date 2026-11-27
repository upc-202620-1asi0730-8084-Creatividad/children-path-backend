using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all subscriptions matching a specific lifecycle status.
/// </summary>
/// <param name="Status">The target <see cref="SubscriptionStatus" />.</param>
public record GetSubscriptionsByStatusQuery(SubscriptionStatus Status);