using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Subscriptions.Domain;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController : ControllerBase
{
    // Mock de Planes
    private static readonly List<Plan> _mockPlans = new()
    {
        new Plan { Id = 1, Name = "Starter", Price = 299, Type = PlanType.Basic },
        new Plan { Id = 2, Name = "Plan Pro", Price = 799, Type = PlanType.Premium },
        new Plan { Id = 3, Name = "Enterprise", Price = 0, Type = PlanType.Enterprise } // Precio 0 = Contactar
    };

    // Mock de Suscripciones
    private static readonly List<Subscription> _mockSubscriptions = new()
    {
        new Subscription 
        { 
            Id = 1, UserId = 1, PlanId = 2, 
            StartDate = new DateTime(2026, 1, 15), 
            EndDate = new DateTime(2026, 8, 15), 
            Status = SubscriptionStatus.Active,
            Invoices = new List<Invoice>
            {
                new Invoice 
                { 
                    Id = 1, InvoiceNumber = "INV-001", Amount = 799, IssuedAt = new DateTime(2026, 7, 15), Status = InvoiceStatus.Paid,
                    Payments = new List<Payment> 
                    { 
                        new Payment { Id = 1, TransactionId = "TXN-123", Amount = 799, Method = PaymentMethod.CreditCard, PaidAt = new DateTime(2026, 7, 15) } 
                    }
                }
            }
        }
    };

    [HttpGet("plans")]
    public IActionResult GetPlans() => Ok(_mockPlans);

    [HttpGet]
    public IActionResult GetSubscriptions() => Ok(_mockSubscriptions);

    [HttpPost]
    public IActionResult Subscribe([FromBody] CreateSubscriptionResource resource)
    {
        // Validar que el plan exista
        var plan = _mockPlans.FirstOrDefault(p => p.Id == resource.PlanId);
        if (plan == null) return NotFound("Plan no encontrado");

        // Validar el método de pago
        if (!Enum.TryParse<PaymentMethod>(resource.PaymentMethod, true, out var paymentMethod))
            return BadRequest("Método de pago inválido");

        var newSubId = _mockSubscriptions.Any() ? _mockSubscriptions.Max(s => s.Id) + 1 : 1;
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(resource.DurationInMonths);

        // Crear la suscripción
        var subscription = new Subscription
        {
            Id = newSubId,
            UserId = resource.UserId,
            PlanId = plan.Id,
            StartDate = startDate,
            EndDate = endDate,
            Status = SubscriptionStatus.Active,
            Plan = plan
        };

        // Simular la generación automática de la primera factura y pago
        var invoice = new Invoice
        {
            Id = 1, 
            InvoiceNumber = $"INV-{newSubId:D3}",
            Amount = plan.Price,
            Status = InvoiceStatus.Paid
        };

        invoice.Payments.Add(new Payment
        {
            Id = 1,
            TransactionId = $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            Amount = plan.Price,
            Method = paymentMethod
        });

        subscription.Invoices.Add(invoice);
        _mockSubscriptions.Add(subscription);

        return CreatedAtAction(nameof(GetSubscriptions), new { id = subscription.Id }, subscription);
    }
}