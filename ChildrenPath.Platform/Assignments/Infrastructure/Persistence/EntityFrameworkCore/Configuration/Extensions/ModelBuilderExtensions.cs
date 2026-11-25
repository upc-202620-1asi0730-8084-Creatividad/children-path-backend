using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Assignments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration extensions for the Assignments bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies Fluent API configuration for <see cref="Assignment" />.
    /// </summary>
    /// <param name="builder">The EF Core <see cref="ModelBuilder" /> instance.</param>
    public static void ApplyAssignmentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Assignment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();

            entity.Property(a => a.StudentCode).IsRequired().HasMaxLength(30);
            entity.Property(a => a.StudentName).IsRequired().HasMaxLength(120);
            entity.Property(a => a.Grade).IsRequired().HasMaxLength(60);
            entity.Property(a => a.GuardianName).IsRequired().HasMaxLength(120);

            entity.Property(a => a.RouteCode).IsRequired().HasMaxLength(30);
            entity.Property(a => a.RouteName).IsRequired().HasMaxLength(120);
            entity.Property(a => a.VehiclePlate).IsRequired().HasMaxLength(30);
            entity.Property(a => a.DriverName).IsRequired().HasMaxLength(120);

            entity.Property(a => a.PickupPoint).IsRequired().HasMaxLength(150);
            entity.Property(a => a.PickupWindow).IsRequired().HasMaxLength(50);
            entity.Property(a => a.Notes).HasMaxLength(500);

            entity.Property(a => a.CapacityUsage).IsRequired();
            entity.Property(a => a.ValidationScore).IsRequired();
            entity.Property(a => a.LastUpdated).IsRequired();

            entity.Property(a => a.Shift)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(a => a.Validation)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            entity.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}