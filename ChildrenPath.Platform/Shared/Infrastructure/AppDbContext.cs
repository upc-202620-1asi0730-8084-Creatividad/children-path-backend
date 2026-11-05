using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Iam.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Shared.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<Company> Companies { get; set; }
    public DbSet<SchoolContract> SchoolContracts { get; set; }
    public DbSet<CompanyMember> CompanyMembers { get; set; }
    public DbSet<ComplianceItem> ComplianceItems { get; set; }
    public DbSet<CompanyActivity> CompanyActivities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Vehicle>().Property(v => v.Plate).IsRequired().HasMaxLength(20);

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
    }
}
