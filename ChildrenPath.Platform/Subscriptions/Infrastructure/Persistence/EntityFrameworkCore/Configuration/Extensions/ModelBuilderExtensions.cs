using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration extensions for the Subscriptions bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies Fluent API configuration for <see cref="Plan" />, <see cref="Subscription" />,
    ///     <see cref="Invoice" />, <see cref="Payment" />, and <see cref="SavedPaymentMethod" />.
    /// </summary>
    /// <param name="builder">The EF Core <see cref="ModelBuilder" /> instance.</param>
    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Plan>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            entity.Property(p => p.Code).IsRequired().HasMaxLength(40);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Currency).IsRequired().HasMaxLength(10);
            entity.Property(p => p.Interval).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(p => p.IsFeatured).IsRequired();
            entity.Property(p => p.IsActive).IsRequired();
            entity.Property(p => p.FeaturesJson).IsRequired().HasMaxLength(2000);
        });

        builder.Entity<Subscription>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();

            entity.Property(s => s.UserId).IsRequired();
            entity.Property(s => s.PlanId).IsRequired();
            entity.Property(s => s.StartDate).IsRequired();
            entity.Property(s => s.EndDate).IsRequired();
            entity.Property(s => s.Status).IsRequired().HasConversion<string>().HasMaxLength(30);

            entity.Property(s => s.ContractOwner).IsRequired().HasMaxLength(120);
            entity.Property(s => s.BillingEmail).IsRequired().HasMaxLength(150);
            entity.Property(s => s.RenewalMode).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(s => s.SupportContact).IsRequired().HasMaxLength(150);
            entity.Property(s => s.PlanScope).HasMaxLength(300);

            entity.Property(s => s.VehiclesUsed).IsRequired();
            entity.Property(s => s.StudentsUsed).IsRequired();
            entity.Property(s => s.UsersUsed).IsRequired();

            entity.HasOne(s => s.Plan)
                .WithMany()
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Invoices)
                .WithOne()
                .HasForeignKey(i => i.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.PaymentMethods)
                .WithOne()
                .HasForeignKey(pm => pm.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedOnAdd();

            entity.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(40);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Amount).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(i => i.Currency).IsRequired().HasMaxLength(10);
            entity.Property(i => i.IssuedAt).IsRequired();
            entity.Property(i => i.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(i => i.PaymentMethodSummary).HasMaxLength(100);
            entity.Property(i => i.InvoiceUrl).HasMaxLength(300);

            entity.HasMany(i => i.Payments)
                .WithOne()
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            entity.Property(p => p.TransactionId).IsRequired().HasMaxLength(60);
            entity.Property(p => p.Amount).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(p => p.Currency).IsRequired().HasMaxLength(10);
            entity.Property(p => p.Method).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(p => p.PaidAt).IsRequired();
            entity.Property(p => p.IsRefunded).IsRequired();
        });

        builder.Entity<SavedPaymentMethod>(entity =>
        {
            entity.HasKey(pm => pm.Id);
            entity.Property(pm => pm.Id).ValueGeneratedOnAdd();

            entity.Property(pm => pm.Brand).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(pm => pm.Last4).IsRequired().HasMaxLength(4);
            entity.Property(pm => pm.Holder).IsRequired().HasMaxLength(120);
            entity.Property(pm => pm.ExpiresOn).IsRequired().HasMaxLength(10);
            entity.Property(pm => pm.IsPrimary).IsRequired();
        });
    }
}