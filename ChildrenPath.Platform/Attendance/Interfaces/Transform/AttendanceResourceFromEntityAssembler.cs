using ChildrenPath.Platform.Attendance.Interfaces.Resources;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="AttendanceResource" /> instances from <see cref="AttendanceEntity" /> aggregates.
/// </summary>
public static class AttendanceResourceFromEntityAssembler
{
    /// <summary>
    ///     Maps an <see cref="AttendanceEntity" /> aggregate to an <see cref="AttendanceResource" /> DTO.
    /// </summary>
    /// <param name="entity">The domain attendance aggregate to map.</param>
    /// <returns>The mapped <see cref="AttendanceResource" />.</returns>
    public static AttendanceResource ToResourceFromEntity(AttendanceEntity entity)
    {
        var boarding = entity.BoardingRecord is not null
            ? new BoardingRecordResource(
                entity.BoardingRecord.Id,
                entity.BoardingRecord.BoardedAt,
                entity.BoardingRecord.Location)
            : null;

        var absence = entity.AbsenceRecord is not null
            ? new AbsenceRecordResource(
                entity.AbsenceRecord.Id,
                entity.AbsenceRecord.Reason,
                entity.AbsenceRecord.Justified)
            : null;

        return new AttendanceResource(
            entity.Id,
            entity.StudentId,
            entity.TripId,
            entity.Status.ToString(),
            entity.Timestamp,
            entity.DroppedOffAt,
            boarding,
            absence);
    }
}