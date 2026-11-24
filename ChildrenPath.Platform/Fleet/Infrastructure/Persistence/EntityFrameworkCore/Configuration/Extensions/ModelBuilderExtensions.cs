using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        // Vehicle Configuration
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Vehicle>().Property(v => v.Plate).IsRequired().HasMaxLength(20);
        builder.Entity<Vehicle>().Property(v => v.Code).IsRequired().HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.Brand).IsRequired().HasMaxLength(100);
        builder.Entity<Vehicle>().Property(v => v.Model).IsRequired().HasMaxLength(100);
        builder.Entity<Vehicle>().Property(v => v.Status).IsRequired().HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.EnergyType).HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.OwnershipType).HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.DriverName).HasMaxLength(120);
        builder.Entity<Vehicle>().Property(v => v.RouteName).HasMaxLength(150);

        // Vehicle -> Documents (1 to Many)
        builder.Entity<Vehicle>()
            .HasMany(v => v.Documents)
            .WithOne()
            .HasForeignKey(d => d.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle -> MaintenanceRecords (1 to Many)
        builder.Entity<Vehicle>()
            .HasMany(v => v.MaintenanceRecords)
            .WithOne()
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // TransportFleet Configuration
        builder.Entity<TransportFleet>().HasKey(f => f.Id);
        builder.Entity<TransportFleet>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<TransportFleet>().Property(f => f.Name).IsRequired().HasMaxLength(150);
        builder.Entity<TransportFleet>().Property(f => f.Description).HasMaxLength(500);

        // TransportFleet -> Vehicles (1 to Many)
        builder.Entity<TransportFleet>()
            .HasMany(f => f.Vehicles)
            .WithOne(v => v.Fleet)
            .HasForeignKey(v => v.FleetId)
            .OnDelete(DeleteBehavior.SetNull);

        // VehicleDocument Configuration
        builder.Entity<VehicleDocument>().HasKey(d => d.Id);
        builder.Entity<VehicleDocument>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<VehicleDocument>().Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
        builder.Entity<VehicleDocument>().Property(d => d.DocumentNumber).IsRequired().HasMaxLength(100);

        // MaintenanceRecord Configuration
        builder.Entity<MaintenanceRecord>().HasKey(m => m.Id);
        builder.Entity<MaintenanceRecord>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MaintenanceRecord>().Property(m => m.MaintenanceType).IsRequired().HasMaxLength(100);
        builder.Entity<MaintenanceRecord>().Property(m => m.Description).HasMaxLength(500);
        builder.Entity<MaintenanceRecord>().Property(m => m.PerformedBy).HasMaxLength(150);
        builder.Entity<MaintenanceRecord>().Property(m => m.Cost).HasColumnType("decimal(18,2)");

        // MaintenanceAlert Configuration
        builder.Entity<MaintenanceAlert>().HasKey(a => a.Id);
        builder.Entity<MaintenanceAlert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MaintenanceAlert>().Property(a => a.VehicleCode).IsRequired().HasMaxLength(50);
        builder.Entity<MaintenanceAlert>().Property(a => a.Plate).IsRequired().HasMaxLength(20);
        builder.Entity<MaintenanceAlert>().Property(a => a.Title).IsRequired().HasMaxLength(150);
        builder.Entity<MaintenanceAlert>().Property(a => a.DueDate).IsRequired().HasMaxLength(50);
        builder.Entity<MaintenanceAlert>().Property(a => a.Priority).IsRequired().HasMaxLength(30);
        builder.Entity<MaintenanceAlert>().Property(a => a.Description).HasMaxLength(500);
    }
}
