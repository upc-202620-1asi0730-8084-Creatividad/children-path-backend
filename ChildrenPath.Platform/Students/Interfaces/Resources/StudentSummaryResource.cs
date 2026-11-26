namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record StudentSummaryResource(
    int TotalStudents,
    int ActiveStudents,
    int AssignedStudents,
    int UnassignedStudents,
    int GuardianVerified,
    int AttendanceReliability);