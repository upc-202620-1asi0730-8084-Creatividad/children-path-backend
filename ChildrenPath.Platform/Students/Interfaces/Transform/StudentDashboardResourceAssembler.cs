using System.Globalization;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Domain.Model.ReadModels;
using ChildrenPath.Platform.Students.Interfaces.Resources;

namespace ChildrenPath.Platform.Students.Interfaces.Transform;

public static class StudentDashboardResourceAssembler
{
    public static StudentSummaryResource ToSummaryResource(StudentSummary summary)
    {
        return new StudentSummaryResource(
            summary.TotalStudents,
            summary.ActiveStudents,
            summary.AssignedStudents,
            summary.UnassignedStudents,
            summary.GuardianVerified,
            summary.AttendanceReliability);
    }

    public static StudentReviewResource ToReviewResource(StudentReview review)
    {
        return new StudentReviewResource(
            review.Id,
            review.StudentCode,
            review.StudentName,
            review.Title,
            review.Description,
            review.DueDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty,
            review.Priority);
    }

    public static StudentActivityResource ToActivityResource(StudentActivity activity)
    {
        return new StudentActivityResource(
            activity.Id.ToString(CultureInfo.InvariantCulture),
            activity.OccurredAt.ToLocalTime().ToString("h:mm tt", CultureInfo.InvariantCulture),
            activity.Title,
            activity.Description,
            activity.StudentName,
            activity.Status);
    }
}