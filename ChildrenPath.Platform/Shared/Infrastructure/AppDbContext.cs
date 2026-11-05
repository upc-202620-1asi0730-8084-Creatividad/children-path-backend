using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Students.Domain;
using ChildrenPath.Platform.Profiles.Domain;
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
    }
}
