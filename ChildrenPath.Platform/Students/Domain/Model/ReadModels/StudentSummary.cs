namespace ChildrenPath.Platform.Students.Domain.Model.ReadModels;

public record StudentSummary(
    int TotalStudents,
    int ActiveStudents,
    int AssignedStudents,
    int UnassignedStudents,
    int GuardianVerified,
    int AttendanceReliability);