using ChildrenPath.Platform.Dashboard.Application.QueryServices;
using ChildrenPath.Platform.Dashboard.Domain.Model.Errors;
using ChildrenPath.Platform.Dashboard.Domain.Model.Queries;
using ChildrenPath.Platform.Dashboard.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Dashboard.Interfaces.Resources;
using ChildrenPath.Platform.Dashboard.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Dashboard.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController(IDashboardQueryService dashboardQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("get dashboard by role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DashboardResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] string role = "company",
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<DashboardType>(role, true, out var type))
        {
            return ResultActionResultAssembler.ToActionResult(
                this,
                Result<object>.Failure(DashboardErrors.InvalidRole),
                _ => Ok());
        }

        var result = await dashboardQueryService.Handle(new GetDashboardForRoleQuery(type), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            snapshot => Ok(DashboardResourceFromSnapshotAssembler.ToResourceFromSnapshot(snapshot)));
    }
}