using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Students.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyStudentsConfiguration(this ModelBuilder builder)
    {
        // Student Configuration
        builder.Entity<Student>().HasKey(s => s.Id);
        builder.Entity<Student>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Student>().Property(s => s.Code).IsRequired().HasMaxLength(20);
        builder.Entity<Student>().HasIndex(s => s.Code).IsUnique();
        builder.Entity<Student>().Property(s => s.FirstName).IsRequired().HasMaxLength(100);
        builder.Entity<Student>().Property(s => s.LastName).IsRequired().HasMaxLength(100);
        builder.Entity<Student>().Property(s => s.Grade).IsRequired().HasMaxLength(50);
        builder.Entity<Student>().Property(s => s.SchoolName).IsRequired().HasMaxLength(150);
        builder.Entity<Student>().Property(s => s.Status).IsRequired().HasMaxLength(20);
        builder.Entity<Student>().Property(s => s.AuthorizationStatus).IsRequired().HasMaxLength(20);
        builder.Entity<Student>().Property(s => s.LastAttendanceStatus).IsRequired().HasMaxLength(30);
        builder.Entity<Student>().Property(s => s.RouteName).HasMaxLength(150);
        builder.Entity<Student>().Property(s => s.RouteCode).HasMaxLength(30);
        builder.Entity<Student>().Property(s => s.AssignedVehicle).HasMaxLength(30);
        builder.Entity<Student>().Property(s => s.AssignedDriver).HasMaxLength(100);
        builder.Entity<Student>().Property(s => s.DropOffPoint).HasMaxLength(250);
        builder.Entity<Student>().Property(s => s.PickupWindow).HasMaxLength(50);
        builder.Entity<Student>().Property(s => s.EmergencyContact).HasMaxLength(50);
        builder.Entity<Student>().Property(s => s.Notes).HasMaxLength(500);

        // Student -> Guardians (1 to Many)
        builder.Entity<Student>()
            .HasMany(s => s.Guardians)
            .WithOne()
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student -> PickupPoint (1 to 1)
        builder.Entity<Student>()
            .HasOne(s => s.PickupPoint)
            .WithOne()
            .HasForeignKey<PickupPoint>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student -> MedicalNotes (1 to Many)
        builder.Entity<Student>()
            .HasMany(s => s.MedicalNotes)
            .WithOne()
            .HasForeignKey(m => m.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guardian Configuration
        builder.Entity<Guardian>().HasKey(g => g.Id);
        builder.Entity<Guardian>().Property(g => g.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Guardian>().Property(g => g.FullName).IsRequired().HasMaxLength(100);
        builder.Entity<Guardian>().Property(g => g.Phone).IsRequired().HasMaxLength(50);
        builder.Entity<Guardian>().Property(g => g.Email).HasMaxLength(150);
        builder.Entity<Guardian>().Property(g => g.Relationship).HasMaxLength(40);

        // PickupPoint Configuration
        builder.Entity<PickupPoint>().HasKey(p => p.Id);
        builder.Entity<PickupPoint>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PickupPoint>().Property(p => p.Address).IsRequired().HasMaxLength(250);

        // MedicalNote Configuration
        builder.Entity<MedicalNote>().HasKey(m => m.Id);
        builder.Entity<MedicalNote>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MedicalNote>().Property(m => m.Description).IsRequired().HasMaxLength(500);
        builder.Entity<MedicalNote>().Property(m => m.Severity).HasConversion<string>().IsRequired();

        // StudentActivity Configuration
        builder.Entity<StudentActivity>().HasKey(activity => activity.Id);
        builder.Entity<StudentActivity>().Property(activity => activity.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<StudentActivity>().Property(activity => activity.StudentCode).IsRequired().HasMaxLength(20);
        builder.Entity<StudentActivity>().Property(activity => activity.StudentName).IsRequired().HasMaxLength(200);
        builder.Entity<StudentActivity>().Property(activity => activity.Title).IsRequired().HasMaxLength(150);
        builder.Entity<StudentActivity>().Property(activity => activity.Description).IsRequired().HasMaxLength(500);
        builder.Entity<StudentActivity>().HasIndex(activity => activity.OccurredAt);
    }
}
