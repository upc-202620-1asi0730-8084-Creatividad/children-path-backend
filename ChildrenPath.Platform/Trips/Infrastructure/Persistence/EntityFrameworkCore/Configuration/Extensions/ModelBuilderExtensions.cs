using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Trips.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTripsConfiguration(this ModelBuilder builder)
    {
        // Configuracion de Trip
        builder.Entity<Trip>().HasKey(t => t.Id);
        builder.Entity<Trip>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Entity<Trip>().Property(t => t.Code).IsRequired().HasMaxLength(50);
        builder.Entity<Trip>().Property(t => t.RouteName).IsRequired().HasMaxLength(150);
        builder.Entity<Trip>().Property(t => t.VehiclePlate).IsRequired().HasMaxLength(20);
        builder.Entity<Trip>().Property(t => t.DriverName).IsRequired().HasMaxLength(120);
        builder.Entity<Trip>().Property(t => t.School).IsRequired().HasMaxLength(150);
        builder.Entity<Trip>().Property(t => t.District).IsRequired().HasMaxLength(100);
        builder.Entity<Trip>().Property(t => t.StartTime).IsRequired().HasMaxLength(20);
        builder.Entity<Trip>().Property(t => t.EstimatedEndTime).IsRequired().HasMaxLength(20);

        // Guardar enums como texto
        builder.Entity<Trip>().Property(t => t.Shift).HasConversion<string>().IsRequired();
        builder.Entity<Trip>().Property(t => t.Status).HasConversion<string>().IsRequired();

        // Relacion: un viaje tiene muchas Paradas (RouteStops)
        builder.Entity<Trip>()
            .HasMany(t => t.Stops)
            .WithOne()
            .HasForeignKey(s => s.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion: un viaje tiene muchos IncidentLogs
        builder.Entity<Trip>()
            .HasMany(t => t.IncidentLogs)
            .WithOne()
            .HasForeignKey(l => l.TripId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configuracion de RouteStop
        builder.Entity<RouteStop>().HasKey(s => s.Id);
        builder.Entity<RouteStop>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<RouteStop>().Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Entity<RouteStop>().Property(s => s.Address).IsRequired().HasMaxLength(250);
        builder.Entity<RouteStop>().Property(s => s.ScheduledTime).IsRequired().HasMaxLength(20);
        builder.Entity<RouteStop>().Property(s => s.Status).HasConversion<string>().IsRequired();
        
        // Configuracion de TripIncidentLog
        builder.Entity<TripIncidentLog>().HasKey(l => l.Id);
        builder.Entity<TripIncidentLog>().Property(l => l.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<TripIncidentLog>().Property(l => l.Description).IsRequired().HasMaxLength(500);
    }
}