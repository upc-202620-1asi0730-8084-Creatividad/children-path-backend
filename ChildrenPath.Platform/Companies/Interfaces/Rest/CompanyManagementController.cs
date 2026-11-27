using System.Net.Mime;
using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Application.QueryServices;
using ChildrenPath.Platform.Companies.Domain.Model.Queries;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest;

[ApiController]
[Route("api/v1/companies/management")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Company Management")]
public class CompanyManagementController(
    ICompanyCommandService companyCommandService,
    ICompanyQueryService companyQueryService) : ControllerBase
{
    [HttpGet("dashboard")]
    [EndpointSummary("get full company management dashboard data")]
    [ProducesResponseType(typeof(CompanyManagementDashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDashboard([FromQuery] int? companyId, CancellationToken cancellationToken)
    {
        var query = new GetCompanyManagementDashboardQuery(companyId);
        var dashboardData = await companyQueryService.Handle(query, cancellationToken);
        
        if (dashboardData == null) return NotFound();

        var resource = CompanyManagementDashboardResourceAssembler.ToResourceFromData(dashboardData);
        return Ok(resource);
    }

    [HttpPatch("compliance/{id:int}/status")]
    [EndpointSummary("update compliance item status")]
    [ProducesResponseType(typeof(ComplianceItemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateComplianceStatus(int id, [FromBody] UpdateComplianceStatusResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateComplianceItemStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await companyCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updatedItem => Ok(ComplianceItemResourceFromEntityAssembler.ToResourceFromEntity(updatedItem)));
    }
}