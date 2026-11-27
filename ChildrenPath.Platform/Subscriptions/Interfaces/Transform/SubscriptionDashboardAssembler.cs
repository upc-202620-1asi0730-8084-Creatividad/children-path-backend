using System.Globalization;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Assembles frontend dashboard DTOs matching the Vue Subscriptions module contracts.
/// </summary>
public static class SubscriptionDashboardAssembler
{
    public static CurrentSubscriptionDashboardResource ToCurrentSubscriptionResource(Subscription entity)
    {
        var planCode = string.IsNullOrWhiteSpace(entity.Plan?.Code) ? "plan" : entity.Plan.Code;

        return new CurrentSubscriptionDashboardResource(
            Id: $"sub-{planCode}-{entity.StartDate.Year}",
            PlanName: entity.Plan?.Name ?? "Plan Pro",
            Status: SubscriptionEnumMapper.ToFrontendSubscriptionStatus(entity.Status),
            Price: entity.Plan?.Price ?? 0m,
            Currency: entity.Plan?.Currency ?? "USD",
            Interval: entity.Plan?.Interval ?? "month",
            RenewsOn: entity.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            StartedOn: entity.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ContractOwner: entity.ContractOwner,
            BillingEmail: entity.BillingEmail,
            RenewalMode: SubscriptionEnumMapper.ToFrontendRenewalMode(entity.RenewalMode),
            SupportContact: entity.SupportContact,
            PlanScope: entity.PlanScope,
            VehicleLimit: entity.Plan?.VehicleLimit,
            StudentLimit: entity.Plan?.StudentLimit,
            UserLimit: entity.Plan?.UserLimit,
            VehiclesUsed: entity.VehiclesUsed,
            StudentsUsed: entity.StudentsUsed,
            UsersUsed: entity.UsersUsed);
    }

    public static IEnumerable<SubscriptionPlanDashboardResource> ToPlanDashboardResources(
        IEnumerable<Plan> plans,
        int? currentPlanId = null)
    {
        return plans.Select(plan => ToPlanDashboardResource(plan, currentPlanId));
    }

    public static SubscriptionPlanDashboardResource ToPlanDashboardResource(Plan plan, int? currentPlanId = null)
    {
        var isCurrent = currentPlanId.HasValue
            ? plan.Id == currentPlanId.Value
            : plan.IsFeatured;

        string actionType;
        if (plan.Price is null || plan.Type == PlanType.Enterprise)
            actionType = "contact";
        else if (isCurrent)
            actionType = "current";
        else
            actionType = "upgrade";

        var features = PlanResourceFromEntityAssembler.DeserializeFeatures(plan.FeaturesJson);

        return new SubscriptionPlanDashboardResource(
            Id: plan.Code,
            Name: plan.Name,
            Label: plan.Code,
            Price: plan.Price,
            Currency: plan.Currency,
            Interval: plan.Interval,
            Target: plan.Code,
            Featured: isCurrent || plan.IsFeatured,
            ActionType: actionType,
            Features: features);
    }

    public static IEnumerable<PaymentMethodDashboardResource> ToPaymentMethodDashboardResources(
        IEnumerable<SavedPaymentMethod> paymentMethods)
    {
        return paymentMethods
            .OrderByDescending(pm => pm.IsPrimary)
            .ThenBy(pm => pm.Id)
            .Select(ToPaymentMethodDashboardResource);
    }

    public static PaymentMethodDashboardResource ToPaymentMethodDashboardResource(SavedPaymentMethod entity)
    {
        var brand = SubscriptionEnumMapper.ToFrontendCardBrand(entity.Brand);
        return new PaymentMethodDashboardResource(
            Id: $"pm-{brand}-{entity.Id}",
            Brand: brand,
            Last4: entity.Last4,
            Holder: entity.Holder,
            ExpiresOn: entity.ExpiresOn,
            Primary: entity.IsPrimary);
    }

    public static IEnumerable<BillingRecordDashboardResource> ToBillingHistoryResources(
        IEnumerable<Invoice> invoices)
    {
        return invoices
            .OrderByDescending(i => i.IssuedAt)
            .ThenByDescending(i => i.Id)
            .Select(ToBillingRecordResource);
    }

    public static BillingRecordDashboardResource ToBillingRecordResource(Invoice entity)
    {
        var id = string.IsNullOrWhiteSpace(entity.InvoiceNumber)
            ? $"inv-{entity.Id:D3}"
            : entity.InvoiceNumber.ToLowerInvariant();

        var method = !string.IsNullOrWhiteSpace(entity.PaymentMethodSummary)
            ? entity.PaymentMethodSummary
            : entity.Payments.FirstOrDefault()?.Method.ToString() ?? "Card";

        return new BillingRecordDashboardResource(
            Id: id,
            Date: entity.IssuedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Description: entity.Description,
            Amount: entity.Amount,
            Currency: entity.Currency,
            Method: method,
            Status: SubscriptionEnumMapper.ToFrontendInvoiceStatus(entity.Status),
            InvoiceUrl: entity.InvoiceUrl);
    }
}