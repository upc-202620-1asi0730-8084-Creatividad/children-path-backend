using System.Net.Mime;
using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Application.QueryServices;
using ChildrenPath.Platform.Companies.Domain.Model.Queries;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Company Management")]
public class CompanyManagementController(
    ICompanyCommandService companyCommandService,
    ICompanyQueryService companyQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get full company management dashboard data")]
    [ProducesResponseType(typeof(CompanyManagementDashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDashboard([FromQuery] int? companyId, CancellationToken cancellationToken)
    {
        var query = new GetCompanyManagementDashboardQuery(companyId);
        var dashboardData = await companyQueryService.Handle(query, cancellationToken);
        if (dashboardData == null)
        {
            return NotFound(new { message = "Company dashboard data not found." });
        }

        var resource = CompanyManagementDashboardResourceAssembler.ToResourceFromData(dashboardData);
        return Ok(resource);
    }

    [HttpPatch("compliance/{id:int}/status")]
    [EndpointSummary("Update compliance item status")]
    [ProducesResponseType(typeof(ComplianceItemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateComplianceStatus(int id, [FromBody] UpdateComplianceStatusResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateComplianceItemStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var item = await companyCommandService.Handle(command, cancellationToken);
        if (item == null)
        {
            return NotFound(new { message = $"Compliance item with id {id} not found." });
        }

        var itemResource = ComplianceItemResourceFromEntityAssembler.ToResourceFromEntity(item);
        return Ok(itemResource);
    }
}
