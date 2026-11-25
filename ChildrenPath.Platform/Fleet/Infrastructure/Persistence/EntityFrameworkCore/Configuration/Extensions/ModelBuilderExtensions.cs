using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyFleetConfiguration(this ModelBuilder b)
    {
        b.Entity<Vehicle>().HasKey(v => v.Id);
        // Vehicle Configuration
        b.Entity<Vehicle>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        b.Entity<Vehicle>().Property(v => v.Plate).IsRequired().HasMaxLength(20);
        b.Entity<Vehicle>().Property(v => v.Code).IsRequired().HasMaxLength(50);
        b.Entity<Vehicle>().Property(v => v.Brand).IsRequired().HasMaxLength(100);
        b.Entity<Vehicle>().Property(v => v.Model).IsRequired().HasMaxLength(100);
        b.Entity<Vehicle>().Property(v => v.Status).IsRequired().HasMaxLength(50);
        b.Entity<Vehicle>().Property(v => v.EnergyType).HasMaxLength(50);
        b.Entity<Vehicle>().Property(v => v.OwnershipType).HasMaxLength(50);
        b.Entity<Vehicle>().Property(v => v.DriverName).HasMaxLength(120);
        b.Entity<Vehicle>().Property(v => v.RouteName).HasMaxLength(150);

        // Vehicle -> Documents (1 to Many)
        b.Entity<Vehicle>()
            .HasMany(v => v.Documents)
            .WithOne()
            .HasForeignKey(d => d.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle -> MaintenanceRecords (1 to Many)
        b.Entity<Vehicle>()
            .HasMany(v => v.MaintenanceRecords)
            .WithOne()
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // TransportFleet Configuration
        b.Entity<TransportFleet>().HasKey(f => f.Id);
        b.Entity<TransportFleet>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        b.Entity<TransportFleet>().Property(f => f.Name).IsRequired().HasMaxLength(150);
        b.Entity<TransportFleet>().Property(f => f.Description).HasMaxLength(500);

        // TransportFleet -> Vehicles (1 to Many)
        b.Entity<TransportFleet>()
            .HasMany(f => f.Vehicles)
            .WithOne(v => v.Fleet)
            .HasForeignKey(v => v.FleetId)
            .OnDelete(DeleteBehavior.SetNull);

        // VehicleDocument Configuration
        b.Entity<VehicleDocument>().HasKey(d => d.Id);
        b.Entity<VehicleDocument>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        b.Entity<VehicleDocument>().Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
        b.Entity<VehicleDocument>().Property(d => d.DocumentNumber).IsRequired().HasMaxLength(100);

        // MaintenanceRecord Configuration
        b.Entity<MaintenanceRecord>().HasKey(m => m.Id);
        b.Entity<MaintenanceRecord>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        b.Entity<MaintenanceRecord>().Property(m => m.MaintenanceType).IsRequired().HasMaxLength(100);
        b.Entity<MaintenanceRecord>().Property(m => m.Description).HasMaxLength(500);
        b.Entity<MaintenanceRecord>().Property(m => m.PerformedBy).HasMaxLength(150);
        b.Entity<MaintenanceRecord>().Property(m => m.Cost).HasColumnType("decimal(18,2)");

        // MaintenanceAlert Configuration
        b.Entity<MaintenanceAlert>().HasKey(a => a.Id);
        b.Entity<MaintenanceAlert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        b.Entity<MaintenanceAlert>().Property(a => a.VehicleCode).IsRequired().HasMaxLength(50);
        b.Entity<MaintenanceAlert>().Property(a => a.Plate).IsRequired().HasMaxLength(20);
        b.Entity<MaintenanceAlert>().Property(a => a.Title).IsRequired().HasMaxLength(150);
        b.Entity<MaintenanceAlert>().Property(a => a.DueDate).IsRequired().HasMaxLength(50);
        b.Entity<MaintenanceAlert>().Property(a => a.Priority).IsRequired().HasMaxLength(30);
        b.Entity<MaintenanceAlert>().Property(a => a.Description).HasMaxLength(500);
    }
}
