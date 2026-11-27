namespace ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a subscription.
/// </summary>
public enum SubscriptionStatus
{
    Active = 0,
    Pending = 1,
    PastDue = 2,
    Cancelled = 3,
    Expired = 4
}