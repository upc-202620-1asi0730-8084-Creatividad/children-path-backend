using System.Net.Mime;
using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Application.QueryServices;
using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Domain.Model.Queries;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest;

[ApiController]
[Route("api/v1/companies")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Companies")]
public class CompaniesController(
    ICompanyCommandService companyCommandService,
    ICompanyQueryService companyQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("get all companies")]
    [ProducesResponseType(typeof(IEnumerable<CompanyResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCompanies(CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery();
        var companies = await companyQueryService.Handle(query, cancellationToken);
        return Ok(companies.Select(CompanyResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get company by ID")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyById(int id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var company = await companyQueryService.Handle(query, cancellationToken);
        
        if (company == null) return NotFound();

        return Ok(CompanyResourceFromEntityAssembler.ToResourceFromEntity(company));
    }

    [HttpPost]
    [EndpointSummary("create a new company")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyResource resource, CancellationToken cancellationToken)
    {
        var command = CreateCompanyCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await companyCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => 
            {
                var resource = CompanyResourceFromEntityAssembler.ToResourceFromEntity(created);
                return CreatedAtAction(nameof(GetCompanyById), new { id = created.Id }, resource);
            });
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("update an existing company")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateCompanyCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await companyCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(CompanyResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("delete a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        var result = await companyCommandService.HandleDelete(id, cancellationToken);
        
        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }
}