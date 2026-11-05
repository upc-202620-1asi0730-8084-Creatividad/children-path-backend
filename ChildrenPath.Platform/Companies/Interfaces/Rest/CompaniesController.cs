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
[Tags("Companies")]
public class CompaniesController(
    ICompanyCommandService companyCommandService,
    ICompanyQueryService companyQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all companies")]
    [ProducesResponseType(typeof(IEnumerable<CompanyResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCompanies(CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery();
        var companies = await companyQueryService.Handle(query, cancellationToken);
        var resources = companies.Select(CompanyResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get company by ID")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyById(int id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var company = await companyQueryService.Handle(query, cancellationToken);
        if (company == null) return NotFound(new { message = $"Company with id {id} not found." });

        var resource = CompanyResourceFromEntityAssembler.ToResourceFromEntity(company);
        return Ok(resource);
    }

    [HttpPost]
    [EndpointSummary("Create a new company")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateCompanyCommandFromResourceAssembler.ToCommandFromResource(resource);
            var company = await companyCommandService.Handle(command, cancellationToken);
            if (company == null) return BadRequest(new { message = "Could not create company." });

            var createdResource = CompanyResourceFromEntityAssembler.ToResourceFromEntity(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, createdResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update an existing company")]
    [ProducesResponseType(typeof(CompanyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateCompanyCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var updated = await companyCommandService.Handle(command, cancellationToken);
        if (updated == null) return NotFound(new { message = $"Company with id {id} not found." });

        var updatedResource = CompanyResourceFromEntityAssembler.ToResourceFromEntity(updated);
        return Ok(updatedResource);
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        var deleted = await companyCommandService.HandleDelete(id, cancellationToken);
        if (!deleted) return NotFound(new { message = $"Company with id {id} not found." });

        return NoContent();
    }
}
