using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Iam.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Shared.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Vehicle>().Property(v => v.Plate).IsRequired().HasMaxLength(20);

    }
}
