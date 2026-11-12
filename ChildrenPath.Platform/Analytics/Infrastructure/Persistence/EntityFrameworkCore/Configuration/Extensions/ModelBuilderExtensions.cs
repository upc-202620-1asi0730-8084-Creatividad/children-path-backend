using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        // ==========================================
        // Configuración de Report
        // ==========================================
        builder.Entity<Report>().HasKey(r => r.Id);
        builder.Entity<Report>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Report>().Property(r => r.Period).IsRequired().HasMaxLength(100);
        builder.Entity<Report>().Property(r => r.Type).HasConversion<string>().IsRequired();

        // Relación: Report -> Metrics (1 a N)
        builder.Entity<Report>()
            .HasMany(r => r.Metrics)
            .WithOne()
            .HasForeignKey(m => m.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación: Report -> KPIs (1 a N)
        builder.Entity<Report>()
            .HasMany(r => r.Kpis)
            .WithOne()
            .HasForeignKey(k => k.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // ==========================================
        // Configuración de Metric
        // ==========================================
        builder.Entity<Metric>().HasKey(m => m.Id);
        builder.Entity<Metric>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Metric>().Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Metric>().Property(m => m.Unit).IsRequired().HasMaxLength(50);

        // ==========================================
        // Configuración de KPI
        // ==========================================
        builder.Entity<KPI>().HasKey(k => k.Id);
        builder.Entity<KPI>().Property(k => k.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<KPI>().Property(k => k.Code).IsRequired().HasMaxLength(50);
        builder.Entity<KPI>().Property(k => k.Description).IsRequired().HasMaxLength(200);
    }
}