using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;

public partial class Subscription
{
    public void Renew(DateTime newEndDate)
    {
        EndDate = newEndDate;
        Status = SubscriptionStatus.Active;
    }

    public void ChangePlan(int newPlanId, DateTime newEndDate)
    {
        PlanId = newPlanId;
        EndDate = newEndDate;
        Status = SubscriptionStatus.Active;
    }

    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
    }

    public void MarkAsPastDue()
    {
        Status = SubscriptionStatus.PastDue;
    }

    public void MarkAsExpired()
    {
        Status = SubscriptionStatus.Expired;
    }

    public void UpdateUsage(int vehiclesUsed, int studentsUsed, int usersUsed)
    {
        VehiclesUsed = Math.Max(vehiclesUsed, 0);
        StudentsUsed = Math.Max(studentsUsed, 0);
        UsersUsed = Math.Max(usersUsed, 0);
    }

    public Invoice AddInvoice(
        string invoiceNumber,
        string description,
        decimal amount,
        string currency,
        InvoiceStatus status,
        string paymentMethodSummary = "",
        string invoiceUrl = "#")
    {
        var invoice = new Invoice(
            invoiceNumber,
            description,
            amount,
            currency,
            status,
            paymentMethodSummary,
            invoiceUrl);

        Invoices.Add(invoice);
        return invoice;
    }

    public SavedPaymentMethod AddPaymentMethod(
        CardBrand brand,
        string last4,
        string holder,
        string expiresOn,
        bool makePrimary = false)
    {
        var shouldBePrimary = makePrimary || PaymentMethods.Count == 0;
        if (shouldBePrimary)
        {
            foreach (var existing in PaymentMethods)
                existing.UnsetPrimary();
        }

        var method = new SavedPaymentMethod(brand, last4, holder, expiresOn, shouldBePrimary);
        PaymentMethods.Add(method);
        return method;
    }

    public bool SetPrimaryPaymentMethod(int paymentMethodId)
    {
        var target = PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
        if (target is null) return false;

        foreach (var existing in PaymentMethods)
            existing.UnsetPrimary();

        target.SetAsPrimary();
        return true;
    }

    public bool RemovePaymentMethod(int paymentMethodId)
    {
        var target = PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
        if (target is null) return false;

        var wasPrimary = target.IsPrimary;
        PaymentMethods.Remove(target);

        if (wasPrimary && PaymentMethods.Count > 0)
            PaymentMethods[0].SetAsPrimary();

        return true;
    }
}