using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;

/// <summary>
///     Aggregate root representing an organization's or user's active subscription contract,
///     its billing invoices, and saved payment methods.
/// </summary>
public partial class Subscription
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int PlanId { get; private set; }
    public DateTime StartDate { get; private set; } = DateTime.UtcNow;
    public DateTime EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; } = SubscriptionStatus.Active;

    // Contract & billing metadata (consumed by frontend dashboard)
    public string ContractOwner { get; private set; } = "MoviSafe Transport";
    public string BillingEmail { get; private set; } = "operations@movisafe.pe";
    public RenewalMode RenewalMode { get; private set; } = RenewalMode.Assisted;
    public string SupportContact { get; private set; } = "billing@childrenpath.pe";
    public string PlanScope { get; private set; } = "Company fleet contract assigned by Children Path sales team";

    // Current plan resource usage
    public int VehiclesUsed { get; private set; }
    public int StudentsUsed { get; private set; }
    public int UsersUsed { get; private set; }

    // Navigation to Plan
    public Plan Plan { get; private set; } = null!;

    // Composition: Subscription -> Invoices & SavedPaymentMethods
    public List<Invoice> Invoices { get; private set; } = [];
    public List<SavedPaymentMethod> PaymentMethods { get; private set; } = [];

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Subscription()
    {
    }

    public Subscription(
        int userId,
        int planId,
        DateTime startDate,
        DateTime endDate,
        string? contractOwner = null,
        string? billingEmail = null,
        RenewalMode renewalMode = RenewalMode.Assisted,
        string? supportContact = null,
        string? planScope = null,
        int vehiclesUsed = 0,
        int studentsUsed = 0,
        int usersUsed = 1,
        SubscriptionStatus status = SubscriptionStatus.Active)
    {
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        ContractOwner = string.IsNullOrWhiteSpace(contractOwner) ? "MoviSafe Transport" : contractOwner.Trim();
        BillingEmail = string.IsNullOrWhiteSpace(billingEmail) ? "operations@movisafe.pe" : billingEmail.Trim();
        RenewalMode = renewalMode;
        SupportContact = string.IsNullOrWhiteSpace(supportContact) ? "billing@childrenpath.pe" : supportContact.Trim();
        PlanScope = string.IsNullOrWhiteSpace(planScope)
            ? "Company fleet contract assigned by Children Path sales team"
            : planScope.Trim();
        VehiclesUsed = Math.Max(vehiclesUsed, 0);
        StudentsUsed = Math.Max(studentsUsed, 0);
        UsersUsed = Math.Max(usersUsed, 0);
    }
}