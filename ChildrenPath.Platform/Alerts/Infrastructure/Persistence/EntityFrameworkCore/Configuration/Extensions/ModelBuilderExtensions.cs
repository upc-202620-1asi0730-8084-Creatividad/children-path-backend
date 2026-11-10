using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertsConfiguration(this ModelBuilder builder)
    {
        // ==========================================
        // Configuración de Alert
        // ==========================================
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Alert>().Property(a => a.TripId).IsRequired();
        builder.Entity<Alert>().Property(a => a.Message).IsRequired().HasMaxLength(500);
        
        // Guardar enums como texto
        builder.Entity<Alert>().Property(a => a.Type).HasConversion<string>().IsRequired();
        builder.Entity<Alert>().Property(a => a.Priority).HasConversion<string>().IsRequired();
        builder.Entity<Alert>().Property(a => a.Status).HasConversion<string>().IsRequired();

        // Relación: Una Alerta tiene muchas Notificaciones
        builder.Entity<Alert>()
            .HasMany(a => a.Notifications)
            .WithOne()
            .HasForeignKey(n => n.AlertId)
            .OnDelete(DeleteBehavior.Cascade);

        // ==========================================
        // Configuración de Notification
        // ==========================================
        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Notification>().Property(n => n.Recipient).IsRequired().HasMaxLength(200);
        builder.Entity<Notification>().Property(n => n.Channel).HasConversion<string>().IsRequired();
        builder.Entity<Notification>().Property(n => n.Status).HasConversion<string>().IsRequired();

        // Relación: Una Notificación tiene muchos DeliveryLogs
        builder.Entity<Notification>()
            .HasMany(n => n.DeliveryLogs)
            .WithOne()
            .HasForeignKey("NotificationId") // FK en sombra (Shadow Property)
            .OnDelete(DeleteBehavior.Cascade);

        // ==========================================
        // Configuración de DeliveryLog
        // ==========================================
        builder.Entity<DeliveryLog>().HasKey(d => d.Id);
        builder.Entity<DeliveryLog>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<DeliveryLog>().Property(d => d.Response).HasMaxLength(1000);

        // ==========================================
        // Configuración de NotificationTemplate
        // ==========================================
        builder.Entity<NotificationTemplate>().HasKey(t => t.Id);
        builder.Entity<NotificationTemplate>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<NotificationTemplate>().Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Entity<NotificationTemplate>().Property(t => t.Channel).HasConversion<string>().IsRequired();
        
        // ==========================================
        // Configuración de AlertRule
        // ==========================================
        builder.Entity<AlertRule>().HasKey(r => r.Id);
        builder.Entity<AlertRule>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<AlertRule>().Property(r => r.Type).HasConversion<string>().IsRequired();
    }
}