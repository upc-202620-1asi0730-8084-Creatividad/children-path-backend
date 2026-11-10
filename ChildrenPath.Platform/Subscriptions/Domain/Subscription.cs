namespace ChildrenPath.Platform.Subscriptions.Domain;

// --- ENUMS ---
public enum PlanType { Basic, Premium, Enterprise }
public enum SubscriptionStatus { Active, PastDue, Cancelled, Expired }
public enum InvoiceStatus { Draft, Issued, Paid, Voided }
public enum PaymentMethod { CreditCard, DebitCard, BankTransfer, PayPal }

// --- ENTIDADES ---

public class Plan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public PlanType Type { get; set; }
    public bool IsActive { get; set; } = true;

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}

public class Payment
{
    public int Id { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public int InvoiceId { get; set; } // FK

    public bool Process() => true; // Lógica simple para el mock
    public void Refund() { /* Lógica de reembolso */ }
}

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public int SubscriptionId { get; set; } // FK

    // Composición: Un Invoice tiene varios Payments
    public List<Payment> Payments { get; set; } = new();

    public void MarkAsPaid() => Status = InvoiceStatus.Paid;
    public void VoidInvoice() => Status = InvoiceStatus.Voided;
}

public class Subscription
{
    public int Id { get; set; }
    public int UserId { get; set; } // Referencia al contexto IAM (futuro)
    public int PlanId { get; set; } // FK
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    // Propiedad de navegación para el Plan
    public Plan Plan { get; set; } = null!;

    // Composición: Una suscripción genera muchas facturas
    public List<Invoice> Invoices { get; set; } = new();

    public void Renew(DateTime newEndDate)
    {
        EndDate = newEndDate;
        Status = SubscriptionStatus.Active;
    }

    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
    }
}