using ChildrenPath.Platform.Students.Application.QueryServices;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Domain.Model.Queries;
using ChildrenPath.Platform.Students.Domain.Model.ReadModels;
using ChildrenPath.Platform.Students.Domain.Repositories;

namespace ChildrenPath.Platform.Students.Application.Internal.QueryServices;

public class StudentQueryService(
    IStudentRepository studentRepository,
    IStudentActivityRepository activityRepository) : IStudentQueryService
{
    public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken = default)
        => await studentRepository.ListWithRelationsAsync(cancellationToken);

    public async Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken = default)
        => await studentRepository.FindWithRelationsByIdAsync(query.Id, cancellationToken);

    public async Task<StudentSummary> Handle(GetStudentsSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var students = (await studentRepository.ListWithRelationsAsync(cancellationToken)).ToList();
        var total = students.Count;
        var attendanceReliability = total == 0
            ? 0
            : (int)Math.Round(students.Average(student => student.AttendanceRate), MidpointRounding.AwayFromZero);
        var verifiedGuardians = students.Count(student =>
            string.Equals(student.AuthorizationStatus, "verified", StringComparison.OrdinalIgnoreCase));

        return new StudentSummary(
            TotalStudents: total,
            ActiveStudents: students.Count(student => string.Equals(student.Status, "active", StringComparison.OrdinalIgnoreCase)),
            AssignedStudents: students.Count(student => !string.IsNullOrWhiteSpace(student.RouteName)),
            UnassignedStudents: students.Count(student =>
                string.IsNullOrWhiteSpace(student.RouteName) ||
                string.Equals(student.Status, "unassigned", StringComparison.OrdinalIgnoreCase)),
            GuardianVerified: total == 0
                ? 0
                : (int)Math.Round((double)verifiedGuardians * 100 / total, MidpointRounding.AwayFromZero),
            AttendanceReliability: attendanceReliability);
    }

    public async Task<IEnumerable<StudentReview>> Handle(GetStudentReviewsQuery query, CancellationToken cancellationToken = default)
    {
        var students = await studentRepository.ListWithRelationsAsync(cancellationToken);
        var reviews = new List<StudentReview>();

        foreach (var student in students)
        {
            var name = $"{student.FirstName} {student.LastName}".Trim();
            if (string.IsNullOrWhiteSpace(student.PickupPoint?.Address))
            {
                reviews.Add(new StudentReview(
                    $"pickup-{student.Id}", student.Code, name,
                    "Pickup point validation pending",
                    "The student has no confirmed pickup point and cannot be assigned to a route yet.",
                    null, "high"));
            }

            if (student.Guardians.All(guardian => string.IsNullOrWhiteSpace(guardian.Phone)))
            {
                reviews.Add(new StudentReview(
                    $"guardian-{student.Id}", student.Code, name,
                    "Guardian contact information missing",
                    "Add a phone number for at least one guardian before transport service begins.",
                    null, "high"));
            }

            if (!string.Equals(student.AuthorizationStatus, "verified", StringComparison.OrdinalIgnoreCase))
            {
                reviews.Add(new StudentReview(
                    $"authorization-{student.Id}", student.Code, name,
                    "Guardian authorization requires review",
                    "The guardian authorization is not marked as verified.",
                    null, "medium"));
            }
        }

        return reviews;
    }

    public async Task<IEnumerable<StudentActivity>> Handle(
        GetStudentActivitiesQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = Math.Clamp(query.Limit, 0, 100);
        return await activityRepository.ListRecentAsync(limit, cancellationToken);
    }
}