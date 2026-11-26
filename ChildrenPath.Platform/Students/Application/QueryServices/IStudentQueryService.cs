using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Domain.Model.Queries;
using ChildrenPath.Platform.Students.Domain.Model.ReadModels;

namespace ChildrenPath.Platform.Students.Application.QueryServices;

public interface IStudentQueryService
{
    Task<IEnumerable<Student>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken = default);
    Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken = default);
    Task<StudentSummary> Handle(GetStudentsSummaryQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentReview>> Handle(GetStudentReviewsQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentActivity>> Handle(GetStudentActivitiesQuery query, CancellationToken cancellationToken = default);
}