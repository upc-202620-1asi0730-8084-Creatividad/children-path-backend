using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Students.Domain;
using ChildrenPath.Platform.Profiles.Domain;
using ChildrenPath.Platform.Subscriptions.Domain;
using ChildrenPath.Platform.Drivers.Domain;
using ChildrenPath.Platform.Iam.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Shared.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleDocument> VehicleDocuments { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

    public DbSet<Student> Students { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<MedicalNote> MedicalNotes { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverAvailability> DriverAvailabilities { get; set; }
    public DbSet<DriverAssignment> DriverAssignments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración User
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        // Fleet
        modelBuilder.Entity<Vehicle>().HasKey(v => v.Id);
        modelBuilder.Entity<Vehicle>().HasMany(v => v.Documents).WithOne().HasForeignKey(d => d.VehicleId);
        modelBuilder.Entity<Vehicle>().HasMany(v => v.MaintenanceRecords).WithOne().HasForeignKey(m => m.VehicleId);

        // Students
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
        modelBuilder.Entity<Student>().HasMany(s => s.Guardians).WithOne().HasForeignKey(g => g.StudentId);
        modelBuilder.Entity<Student>().HasOne(s => s.PickupPoint).WithOne().HasForeignKey<PickupPoint>(p => p.StudentId);
        modelBuilder.Entity<Student>().HasMany(s => s.MedicalNotes).WithOne().HasForeignKey(m => m.StudentId);
        
        //Profiles
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
        
        // Configuración de Drivers
        modelBuilder.Entity<Driver>().HasKey(d => d.Id);
        // Composición: Driver -> Availabilities
        modelBuilder.Entity<Driver>().HasMany(d => d.Availabilities).WithOne().HasForeignKey(a => a.DriverId).OnDelete(DeleteBehavior.Cascade);
        // Composición: Driver -> Assignments
        modelBuilder.Entity<Driver>().HasMany(d => d.Assignments).WithOne().HasForeignKey(a => a.DriverId).OnDelete(DeleteBehavior.Cascade);
    } 
}

