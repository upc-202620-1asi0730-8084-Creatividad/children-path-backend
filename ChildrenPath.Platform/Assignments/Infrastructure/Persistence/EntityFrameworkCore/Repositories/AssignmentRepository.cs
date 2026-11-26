using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Assignments.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Assignments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core implementation of <see cref="IAssignmentRepository" />.
/// </summary>
/// <param name="context">The application <see cref="AppDbContext" />.</param>
public class AssignmentRepository(AppDbContext context)
    : BaseRepository<Assignment>(context), IAssignmentRepository
{
    /// <inheritdoc />
    public override async Task<IEnumerable<Assignment>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Assignment>()
            .OrderByDescending(a => a.LastUpdated)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByStudentCodeAndShiftAsync(
        string studentCode,
        AssignmentShift shift,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = studentCode.Trim().ToLower();
        return await Context.Set<Assignment>()
            .AnyAsync(
                a => a.StudentCode.ToLower() == normalizedCode && a.Shift == shift,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByStudentCodeAndShiftExcludingIdAsync(
        string studentCode,
        AssignmentShift shift,
        int excludeId,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = studentCode.Trim().ToLower();
        return await Context.Set<Assignment>()
            .AnyAsync(
                a => a.Id != excludeId
                     && a.StudentCode.ToLower() == normalizedCode
                     && a.Shift == shift,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> FindByStatusAsync(
        AssignmentStatus status,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Assignment>()
            .Where(a => a.Status == status)
            .OrderByDescending(a => a.LastUpdated)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> FindByShiftAsync(
        AssignmentShift shift,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Assignment>()
            .Where(a => a.Shift == shift)
            .OrderByDescending(a => a.LastUpdated)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Assignment>> FindByStudentCodeAsync(
        string studentCode,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = studentCode.Trim().ToLower();
        return await Context.Set<Assignment>()
            .Where(a => a.StudentCode.ToLower() == normalizedCode)
            .OrderByDescending(a => a.LastUpdated)
            .ToListAsync(cancellationToken);
    }
}