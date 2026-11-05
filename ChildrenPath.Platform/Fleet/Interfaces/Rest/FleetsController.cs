using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Fleet.Interfaces.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class FleetsController : ControllerBase
{
    // 👇 Ahora usamos TransportFleet directamente
    private static readonly List<TransportFleet> _mockFleets = new()
    {
        new TransportFleet { Id = 1, Name = "MoviSafe Transport" }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_mockFleets);

    [HttpPost]
    public IActionResult Create([FromBody] CreateFleetResource resource)
    {
        var newId = _mockFleets.Any() ? _mockFleets.Max(f => f.Id) + 1 : 1;
        
        var fleet = new TransportFleet { Id = newId, Name = resource.Name }; // 👈 Cambiado aquí
        
        _mockFleets.Add(fleet);
        return CreatedAtAction(nameof(GetAll), new { id = fleet.Id }, fleet);
    }
}