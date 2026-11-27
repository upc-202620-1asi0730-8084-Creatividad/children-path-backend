namespace ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents how a subscription contract is renewed.
/// </summary>
public enum RenewalMode
{
    Automatic = 0,
    Manual = 1,
    Assisted = 2
}