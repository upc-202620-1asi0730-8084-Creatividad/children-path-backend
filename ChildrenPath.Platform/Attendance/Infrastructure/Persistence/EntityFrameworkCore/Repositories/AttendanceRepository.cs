using ChildrenPath.Platform.Attendance.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core implementation of <see cref="IAttendanceRepository" />.
/// </summary>
/// <param name="context">The application database context.</param>
public class AttendanceRepository(AppDbContext context)
    : BaseRepository<AttendanceEntity>(context), IAttendanceRepository
{
    /// <summary>
    ///     Finds an attendance record by its unique identifier, including its boarding and absence details.
    /// </summary>
    /// <param name="id">The unique identifier of the attendance record.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching <see cref="AttendanceEntity" /> if found; otherwise, <c>null</c>.</returns>
    public override async Task<AttendanceEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<AttendanceEntity>()
            .Include(a => a.BoardingRecord)
            .Include(a => a.AbsenceRecord)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    /// <summary>
    ///     Lists all attendance records, including their boarding and absence details.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection containing all attendance records.</returns>
    public override async Task<IEnumerable<AttendanceEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<AttendanceEntity>()
            .Include(a => a.BoardingRecord)
            .Include(a => a.AbsenceRecord)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttendanceEntity>> FindByTripIdAsync(
        int tripId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<AttendanceEntity>()
            .Include(a => a.BoardingRecord)
            .Include(a => a.AbsenceRecord)
            .Where(a => a.TripId == tripId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttendanceEntity>> FindByStudentIdAsync(
        int studentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<AttendanceEntity>()
            .Include(a => a.BoardingRecord)
            .Include(a => a.AbsenceRecord)
            .Where(a => a.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByStudentIdAndTripIdAsync(
        int studentId,
        int tripId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<AttendanceEntity>()
            .AnyAsync(a => a.StudentId == studentId && a.TripId == tripId, cancellationToken);
    }
}