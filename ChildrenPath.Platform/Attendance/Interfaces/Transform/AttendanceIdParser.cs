namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

public static class AttendanceIdParser
{
    public static bool TryParse(string rawId, out int attendanceId)
    {
        var cleaned = rawId.Trim();
        if (cleaned.StartsWith("ATT-", StringComparison.OrdinalIgnoreCase))
            cleaned = cleaned[4..];

        return int.TryParse(cleaned, out attendanceId);
    }
}