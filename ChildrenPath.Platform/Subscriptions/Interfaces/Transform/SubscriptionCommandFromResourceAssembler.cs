using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Assembles Subscription domain commands from REST resource payloads.
/// </summary>
public static class SubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(
            resource.UserId,
            resource.PlanId,
            resource.DurationInMonths,
            SubscriptionEnumMapper.ParsePaymentMethod(resource.PaymentMethod),
            resource.ContractOwner,
            resource.BillingEmail,
            SubscriptionEnumMapper.ParseRenewalMode(resource.RenewalMode, defaultToAssistedWhenEmpty: true),
            resource.SupportContact,
            resource.PlanScope,
            resource.VehiclesUsed,
            resource.StudentsUsed,
            resource.UsersUsed);
    }

    public static RenewSubscriptionCommand ToCommandFromResource(int subscriptionId, RenewSubscriptionResource resource)
    {
        return new RenewSubscriptionCommand(
            subscriptionId,
            resource.AdditionalMonths,
            SubscriptionEnumMapper.ParsePaymentMethod(resource.PaymentMethod));
    }

    public static ChangeSubscriptionPlanCommand ToCommandFromResource(
        int subscriptionId,
        ChangeSubscriptionPlanResource resource)
    {
        return new ChangeSubscriptionPlanCommand(
            subscriptionId,
            resource.NewPlanId,
            resource.DurationInMonths,
            SubscriptionEnumMapper.ParsePaymentMethod(resource.PaymentMethod));
    }

    public static UpdateSubscriptionUsageCommand ToCommandFromResource(
        int subscriptionId,
        UpdateSubscriptionUsageResource resource)
    {
        return new UpdateSubscriptionUsageCommand(
            subscriptionId,
            resource.VehiclesUsed,
            resource.StudentsUsed,
            resource.UsersUsed);
    }

    public static PayInvoiceCommand ToCommandFromResource(
        int subscriptionId,
        int invoiceId,
        PayInvoiceResource resource)
    {
        return new PayInvoiceCommand(
            subscriptionId,
            invoiceId,
            SubscriptionEnumMapper.ParsePaymentMethod(resource.Method),
            resource.MethodSummary);
    }

    public static AddPaymentMethodCommand ToCommandFromResource(
        int subscriptionId,
        AddPaymentMethodResource resource)
    {
        return new AddPaymentMethodCommand(
            subscriptionId,
            SubscriptionEnumMapper.ParseCardBrand(resource.Brand),
            resource.CardNumber,
            resource.Holder,
            resource.ExpiresOn,
            resource.MakePrimary);
    }
}