using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Drivers.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DriverRepository(AppDbContext context)
    : BaseRepository<Driver>(context), IDriverRepository
{
    public async Task<Driver?> FindWithRelationsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Drivers
            .Include(driver => driver.Availabilities)
            .Include(driver => driver.Assignments)
            .FirstOrDefaultAsync(driver => driver.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Driver>> ListWithRelationsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Drivers
            .AsNoTracking()
            .Include(driver => driver.Availabilities)
            .Include(driver => driver.Assignments)
            .OrderBy(driver => driver.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        int? excludingId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.ToUpper();
        return await Context.Drivers.AnyAsync(driver =>
            driver.Email.ToUpper() == normalizedEmail &&
            (!excludingId.HasValue || driver.Id != excludingId.Value), cancellationToken);
    }

    public async Task<bool> ExistsByLicenseNumberAsync(
        string licenseNumber,
        int? excludingId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedLicenseNumber = licenseNumber.ToUpper();
        return await Context.Drivers.AnyAsync(driver =>
            driver.LicenseNumber.ToUpper() == normalizedLicenseNumber &&
            (!excludingId.HasValue || driver.Id != excludingId.Value), cancellationToken);
    }
}