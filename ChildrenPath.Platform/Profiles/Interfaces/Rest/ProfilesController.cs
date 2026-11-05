using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Profiles.Domain;
using ChildrenPath.Platform.Profiles.Interfaces.Resources;

namespace ChildrenPath.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfilesController : ControllerBase
{
    private static readonly List<Profile> _mockProfiles = new()
    {
        new Profile 
        { 
            Id = 1, UserId = 1, FirstName = "María", LastName = "López", Phone = "+51 951 111 222", Role = UserRole.Parent,
            Preferences = new Preferences { Language = "es", Timezone = "America/Lima" },
            ParentProfile = new ParentProfile { Relationship = "Mother", ChildrenCount = 1 }
        },
        new Profile 
        { 
            Id = 2, UserId = 2, FirstName = "Carlos", LastName = "Pérez", Phone = "+51 987 221 104", Role = UserRole.Driver,
            Preferences = new Preferences { Language = "es", Timezone = "America/Lima" },
            DriverProfile = new DriverProfile { LicenseNumber = "AIIb-458721", LicenseExpiry = DateTime.Parse("2027-04-15") }
        }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockProfiles);

    [HttpPost]
    public IActionResult Create([FromBody] CreateProfileResource resource)
    {
        var newId = _mockProfiles.Any() ? _mockProfiles.Max(p => p.Id) + 1 : 1;

        // Parseamos el string del rol al Enum
        if (!Enum.TryParse<UserRole>(resource.Role, true, out var role))
            return BadRequest("Rol inválido. Use: Parent, Driver, CompanyAdmin, ChildrenPathAdmin");

        var profile = new Profile
        {
            Id = newId,
            UserId = newId, // Simulamos el ID que vendría de IAM
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            Phone = resource.Phone,
            Role = role,
            Preferences = new Preferences { Language = "es", Timezone = "America/Lima" }
        };

        // Lógica según el rol (Patrón Strategy básico)
        switch (role)
        {
            case UserRole.Parent:
                profile.ParentProfile = new ParentProfile 
                { 
                    Relationship = resource.Relationship ?? "Guardian", 
                    ChildrenCount = 0 
                };
                break;
            case UserRole.Driver:
                profile.DriverProfile = new DriverProfile 
                { 
                    LicenseNumber = resource.LicenseNumber ?? "Pendiente", 
                    LicenseExpiry = resource.LicenseExpiry ?? DateTime.UtcNow.AddYears(1) 
                };
                break;
            case UserRole.CompanyAdmin:
                profile.CompanyProfile = new CompanyProfile 
                { 
                    TaxId = resource.TaxId ?? "Pendiente", 
                    LegalName = resource.LegalName ?? "Pendiente" 
                };
                break;
        }

        _mockProfiles.Add(profile);
        return CreatedAtAction(nameof(GetAll), new { id = profile.Id }, profile);
    }
}