namespace ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

public record CreateSubscriptionResource(
    int UserId,
    int PlanId,
    int DurationInMonths,
    string PaymentMethod
);