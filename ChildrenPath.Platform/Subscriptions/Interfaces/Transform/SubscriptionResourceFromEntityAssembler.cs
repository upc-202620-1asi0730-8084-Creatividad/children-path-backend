using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Assembles standard REST DTOs from <see cref="Subscription" />, <see cref="Invoice" />,
///     <see cref="Payment" />, and <see cref="SavedPaymentMethod" /> domain entities.
/// </summary>
public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        return new SubscriptionResource(
            entity.Id,
            entity.UserId,
            entity.PlanId,
            entity.Plan?.Code ?? string.Empty,
            entity.Plan?.Name ?? string.Empty,
            entity.Plan?.Price,
            entity.Plan?.Currency ?? "USD",
            entity.StartDate,
            entity.EndDate,
            entity.Status.ToString(),
            entity.ContractOwner,
            entity.BillingEmail,
            entity.RenewalMode.ToString(),
            entity.SupportContact,
            entity.PlanScope,
            entity.VehiclesUsed,
            entity.StudentsUsed,
            entity.UsersUsed,
            entity.Invoices
                .OrderByDescending(i => i.IssuedAt)
                .Select(ToInvoiceResourceFromEntity),
            entity.PaymentMethods
                .OrderByDescending(pm => pm.IsPrimary)
                .ThenBy(pm => pm.Id)
                .Select(ToPaymentMethodResourceFromEntity));
    }

    public static InvoiceResource ToInvoiceResourceFromEntity(Invoice entity)
    {
        return new InvoiceResource(
            entity.Id,
            entity.InvoiceNumber,
            entity.Description,
            entity.Amount,
            entity.Currency,
            entity.IssuedAt,
            entity.Status.ToString(),
            entity.PaymentMethodSummary,
            entity.InvoiceUrl,
            entity.Payments.Select(ToPaymentResourceFromEntity));
    }

    public static PaymentResource ToPaymentResourceFromEntity(Payment entity)
    {
        return new PaymentResource(
            entity.Id,
            entity.TransactionId,
            entity.Amount,
            entity.Currency,
            entity.Method.ToString(),
            entity.PaidAt,
            entity.IsRefunded);
    }

    public static SavedPaymentMethodResource ToPaymentMethodResourceFromEntity(SavedPaymentMethod entity)
    {
        return new SavedPaymentMethodResource(
            entity.Id,
            entity.Brand.ToString(),
            entity.Last4,
            entity.Holder,
            entity.ExpiresOn,
            entity.IsPrimary);
    }
}