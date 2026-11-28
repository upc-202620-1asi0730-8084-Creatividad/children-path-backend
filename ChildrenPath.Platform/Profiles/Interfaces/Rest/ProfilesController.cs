using ChildrenPath.Platform.Profiles.Application.CommandServices;
using ChildrenPath.Platform.Profiles.Application.QueryServices;
using ChildrenPath.Platform.Profiles.Domain.Model.Commands;
using ChildrenPath.Platform.Profiles.Domain.Model.Queries;
using ChildrenPath.Platform.Profiles.Interfaces.Resources;
using ChildrenPath.Platform.Profiles.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("create a new profile")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProfileResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateProfileResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateProfileCommand(
            resource.UserId,
            resource.FirstName, resource.LastName, resource.Phone, resource.Role,
            resource.LicenseNumber, resource.LicenseExpiry,
            resource.TaxId, resource.LegalName, resource.Relationship);

        var result = await profileCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, profile =>
        {
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return CreatedAtAction(nameof(GetById), new { id = profileResource.Id }, profileResource);
        });
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get a profile by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(id), cancellationToken);
        if (profile is null) return NotFound();
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    [HttpGet]
    [EndpointSummary("list all profiles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProfileResource>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var profiles = await profileQueryService.Handle(new GetAllProfilesQuery(), cancellationToken);
        return Ok(profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
