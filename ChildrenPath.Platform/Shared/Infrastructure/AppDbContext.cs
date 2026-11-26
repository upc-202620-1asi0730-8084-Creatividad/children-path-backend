using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Profiles.Domain;
using ChildrenPath.Platform.Subscriptions.Domain;
using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Iam.Domain;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Tracking.Domain;

using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Entities;
using ChildrenPath.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Analytics.Domain;
using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Entities;
using ChildrenPath.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Entities;
using ChildrenPath.Platform.Trips.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using ChildrenPath.Platform.Attendance.Domain.Model.Entities;
using ChildrenPath.Platform.Attendance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Shared.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleDocument> VehicleDocuments { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<MaintenanceAlert> MaintenanceAlerts { get; set; }

    public DbSet<Company> Companies { get; set; }
    public DbSet<SchoolContract> SchoolContracts { get; set; }
    public DbSet<CompanyMember> CompanyMembers { get; set; }
    public DbSet<ComplianceItem> ComplianceItems { get; set; }
    public DbSet<CompanyActivity> CompanyActivities { get; set; }

    public DbSet<Student> Students { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<MedicalNote> MedicalNotes { get; set; }
    public DbSet<StudentActivity> StudentActivities { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverAvailability> DriverAvailabilities { get; set; }
    public DbSet<DriverAssignment> DriverAssignments { get; set; }
    public DbSet<DriverActivity> DriverActivities { get; set; }

    public DbSet<TransportFleet> Fleets { get; set; }

    public DbSet<Assignment> Assignments { get; set; }
    
    public DbSet<Trip> Trips { get; set; }
    public DbSet<RouteStop> RouteStops { get; set; }
    public DbSet<TripIncidentLog> TripIncidentLogs { get; set; }
    
    public DbSet<TrackingSession> TrackingSessions { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<ETA> ETAs { get; set; }
    
    public DbSet<AttendanceEntity> Attendances { get; set; } 
    public DbSet<BoardingRecord> BoardingRecords { get; set; }
    public DbSet<AbsenceRecord> AbsenceRecords { get; set; }
    
    public DbSet<Report> Reports { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<KPI> Kpis { get; set; }
    
    // Alerts DbSets
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<DeliveryLog> DeliveryLogs { get; set; }
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    public DbSet<AlertRule> AlertRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración User
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        // Configuración Companies
        modelBuilder.Entity<Company>().HasKey(c => c.Id);
        modelBuilder.Entity<Company>().Property(c => c.LegalName).IsRequired().HasMaxLength(150);
        modelBuilder.Entity<Company>().Property(c => c.CommercialName).IsRequired().HasMaxLength(150);
        modelBuilder.Entity<Company>().Property(c => c.Ruc).IsRequired().HasMaxLength(20);

        modelBuilder.Entity<SchoolContract>().HasKey(sc => sc.Id);
        modelBuilder.Entity<SchoolContract>().Property(sc => sc.SchoolName).IsRequired().HasMaxLength(150);

        modelBuilder.Entity<CompanyMember>().HasKey(cm => cm.Id);
        modelBuilder.Entity<CompanyMember>().Property(cm => cm.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<CompanyMember>().Property(cm => cm.Email).IsRequired().HasMaxLength(100);

        modelBuilder.Entity<ComplianceItem>().HasKey(ci => ci.Id);
        modelBuilder.Entity<ComplianceItem>().Property(ci => ci.Title).IsRequired().HasMaxLength(150);

        modelBuilder.Entity<CompanyActivity>().HasKey(ca => ca.Id);
        modelBuilder.Entity<CompanyActivity>().Property(ca => ca.Title).IsRequired().HasMaxLength(150);
        
        // Fleet
        modelBuilder.ApplyFleetConfiguration();
        
        // Students
        modelBuilder.ApplyStudentsConfiguration();
        
        // Profiles
        modelBuilder.Entity<Profile>().HasKey(p => p.Id);
        modelBuilder.Entity<Profile>().HasOne(p => p.Preferences).WithOne().HasForeignKey<Preferences>(pref => pref.Id);
        modelBuilder.Entity<Profile>().HasOne(p => p.ParentProfile).WithOne().HasForeignKey<ParentProfile>(pp => pp.Id);
        modelBuilder.Entity<Profile>().HasOne(p => p.DriverProfile).WithOne().HasForeignKey<DriverProfile>(dp => dp.Id);
        modelBuilder.Entity<Profile>().HasOne(p => p.CompanyProfile).WithOne().HasForeignKey<CompanyProfile>(cp => cp.Id);
        
        // Configuración de Subscriptions y Payments
        modelBuilder.Entity<Plan>().HasKey(p => p.Id);
        modelBuilder.Entity<Subscription>().HasKey(s => s.Id);
        modelBuilder.Entity<Subscription>().HasOne(s => s.Plan).WithMany().HasForeignKey(s => s.PlanId);
        // Composición: Suscripción -> Facturas
        modelBuilder.Entity<Subscription>().HasMany(s => s.Invoices).WithOne().HasForeignKey(i => i.SubscriptionId).OnDelete(DeleteBehavior.Cascade);
        // Composición: Factura -> Pagos
        modelBuilder.Entity<Invoice>().HasMany(i => i.Payments).WithOne().HasForeignKey(p => p.InvoiceId).OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.ApplyDriversConfiguration();
        
        // Configuración de Assignments
        modelBuilder.ApplyAssignmentsConfiguration();
        
        // Configuración de Tracking
        modelBuilder.Entity<TrackingSession>().HasKey(t => t.Id);
        
        // Composición: TrackingSession -> Locations
        modelBuilder.Entity<TrackingSession>().HasMany(t => t.Locations).WithOne().OnDelete(DeleteBehavior.Cascade);
        // Composición: TrackingSession -> ETAs
        modelBuilder.Entity<TrackingSession>().HasMany(t => t.ETAs).WithOne().OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.ApplyAttendanceConfiguration();
        
        modelBuilder.ApplyAlertsConfiguration();
        modelBuilder.ApplyAnalyticsConfiguration(); 
        
        // Configuración de Analytics
        modelBuilder.Entity<Report>().HasKey(r => r.Id);
        
        // Composición: Report -> Metrics
        modelBuilder.Entity<Report>().HasMany(r => r.Metrics).WithOne().HasForeignKey(m => m.ReportId).OnDelete(DeleteBehavior.Cascade);

        // Composición: Report -> KPIs
        modelBuilder.Entity<Report>().HasMany(r => r.Kpis).WithOne().HasForeignKey(k => k.ReportId).OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.ApplyTripsConfiguration();
        
        
    }
}