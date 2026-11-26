using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Drivers.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyDriversConfiguration(this ModelBuilder builder)
    {
        // Driver Aggregate Root
        builder.Entity<Driver>().HasKey(driver => driver.Id);
        builder.Entity<Driver>().Property(driver => driver.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Driver>().Property(driver => driver.Code).IsRequired().HasMaxLength(20);
        builder.Entity<Driver>().HasIndex(driver => driver.Code).IsUnique();
        builder.Entity<Driver>().Property(driver => driver.FullName).IsRequired().HasMaxLength(150);
        builder.Entity<Driver>().Property(driver => driver.Phone).IsRequired().HasMaxLength(50);
        builder.Entity<Driver>().Property(driver => driver.Email).IsRequired().HasMaxLength(150);
        builder.Entity<Driver>().HasIndex(driver => driver.Email).IsUnique();
        builder.Entity<Driver>().Property(driver => driver.LicenseNumber).IsRequired().HasMaxLength(50);
        builder.Entity<Driver>().HasIndex(driver => driver.LicenseNumber).IsUnique();
        builder.Entity<Driver>().Property(driver => driver.LicenseClass).IsRequired().HasMaxLength(30);
        builder.Entity<Driver>().Property(driver => driver.Status).IsRequired();
        builder.Entity<Driver>().Property(driver => driver.PhotoInitials).HasMaxLength(10);
        builder.Entity<Driver>().Property(driver => driver.AssignedVehicle).HasMaxLength(50);
        builder.Entity<Driver>().Property(driver => driver.AssignedRoute).HasMaxLength(150);
        builder.Entity<Driver>().Property(driver => driver.AvailabilityLabel).HasMaxLength(100);

        // Driver -> Availabilities (1 to Many)
        builder.Entity<Driver>()
            .HasMany(driver => driver.Availabilities)
            .WithOne()
            .HasForeignKey(availability => availability.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        // Driver -> Assignments (1 to Many)
        builder.Entity<Driver>()
            .HasMany(driver => driver.Assignments)
            .WithOne()
            .HasForeignKey(assignment => assignment.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        // DriverAvailability Configuration
        builder.Entity<DriverAvailability>().HasKey(availability => availability.Id);
        builder.Entity<DriverAvailability>().Property(availability => availability.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<DriverAvailability>().Property(availability => availability.DayOfWeek)
            .IsRequired().HasMaxLength(20);

        // DriverAssignment Configuration
        builder.Entity<DriverAssignment>().HasKey(assignment => assignment.Id);
        builder.Entity<DriverAssignment>().Property(assignment => assignment.Id).IsRequired().ValueGeneratedOnAdd();

        // DriverActivity Configuration
        builder.Entity<DriverActivity>().HasKey(activity => activity.Id);
        builder.Entity<DriverActivity>().Property(activity => activity.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<DriverActivity>().Property(activity => activity.DriverName).IsRequired().HasMaxLength(150);
        builder.Entity<DriverActivity>().Property(activity => activity.Title).IsRequired().HasMaxLength(150);
        builder.Entity<DriverActivity>().Property(activity => activity.Description).IsRequired().HasMaxLength(500);
        builder.Entity<DriverActivity>().Property(activity => activity.Status).IsRequired().HasMaxLength(20);
        builder.Entity<DriverActivity>().HasIndex(activity => activity.OccurredAt);
    }
}
