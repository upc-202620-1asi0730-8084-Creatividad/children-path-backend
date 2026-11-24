using ChildrenPath.Platform.Attendance.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
/// <summary>
///     Entity Framework Core model configuration extensions for the Attendance Bounded Context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table keys, constraints, enum conversions, and entity relationships
    ///     for the Attendance aggregate and its child entities.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder" /> instance to configure.</param>
    public static void ApplyAttendanceConfiguration(this ModelBuilder builder)
    {
        // Attendance
        builder.Entity<AttendanceEntity>().HasKey(a => a.Id);
        builder.Entity<AttendanceEntity>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<AttendanceEntity>().Property(a => a.StudentId).IsRequired();
        builder.Entity<AttendanceEntity>().Property(a => a.TripId).IsRequired();
        builder.Entity<AttendanceEntity>().Property(a => a.Timestamp).IsRequired();
        builder.Entity<AttendanceEntity>().Property(a => a.Status).HasConversion<string>().IsRequired();

        // Composición 1:1 Attendance -> BoardingRecord
        builder.Entity<AttendanceEntity>()
            .HasOne(a => a.BoardingRecord)
            .WithOne()
            .HasForeignKey<BoardingRecord>(br => br.AttendanceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composición 1:1 Attendance -> AbsenceRecord
        builder.Entity<AttendanceEntity>()
            .HasOne(a => a.AbsenceRecord)
            .WithOne()
            .HasForeignKey<AbsenceRecord>(ar => ar.AttendanceId)
            .OnDelete(DeleteBehavior.Cascade);

        // BoardingRecord
        builder.Entity<BoardingRecord>().HasKey(br => br.Id);
        builder.Entity<BoardingRecord>().Property(br => br.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<BoardingRecord>().Property(br => br.Location).IsRequired().HasMaxLength(250);
        builder.Entity<BoardingRecord>().Property(br => br.BoardedAt).IsRequired();

        // AbsenceRecord
        builder.Entity<AbsenceRecord>().HasKey(ar => ar.Id);
        builder.Entity<AbsenceRecord>().Property(ar => ar.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<AbsenceRecord>().Property(ar => ar.Reason).IsRequired().HasMaxLength(500);
        builder.Entity<AbsenceRecord>().Property(ar => ar.Justified).IsRequired();
    }
}