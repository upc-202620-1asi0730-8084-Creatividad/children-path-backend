using System.Net.Mime;
using ChildrenPath.Platform.Students.Application.QueryServices;
using ChildrenPath.Platform.Students.Domain.Model.Queries;
using ChildrenPath.Platform.Students.Interfaces.Resources;
using ChildrenPath.Platform.Students.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Students.Interfaces.Rest;

[ApiController]
[Route("api/v1")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Student Management")]
public class StudentManagementController(IStudentQueryService queryService) : ControllerBase
{
    [HttpGet("student-summary")]
    [HttpGet("studentSummary")]
    [EndpointSummary("Get student summary metrics")]
    [ProducesResponseType(typeof(StudentSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await queryService.Handle(new GetStudentsSummaryQuery(), cancellationToken);
        return Ok(StudentDashboardResourceAssembler.ToSummaryResource(summary));
    }

    [HttpGet("student-reviews")]
    [HttpGet("studentReviews")]
    [EndpointSummary("Get student reviews")]
    [ProducesResponseType(typeof(IEnumerable<StudentReviewResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews(CancellationToken cancellationToken)
    {
        var reviews = await queryService.Handle(new GetStudentReviewsQuery(), cancellationToken);
        return Ok(reviews.Select(StudentDashboardResourceAssembler.ToReviewResource));
    }

    [HttpGet("student-activities")]
    [HttpGet("studentActivities")]
    [EndpointSummary("Get recent student activities")]
    [ProducesResponseType(typeof(IEnumerable<StudentActivityResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivities(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var activities = await queryService.Handle(new GetStudentActivitiesQuery(limit), cancellationToken);
        return Ok(activities.Select(StudentDashboardResourceAssembler.ToActivityResource));
    }
}