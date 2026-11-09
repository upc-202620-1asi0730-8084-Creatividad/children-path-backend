using System.Threading.Tasks;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Trips.Domain;
using ChildrenPath.Platform.Trips.Infrastructure.Repositories;
using ChildrenPath.Platform.Trips.Interfaces.Resources;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Trips.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class TripsController(ITripRepository tripRepository, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripResource resource)
    {
       
        var trip = new Trip
        {
            Code = resource.Code,
            RouteName = resource.RouteName,
            VehiclePlate = resource.VehiclePlate,
            DriverName = resource.DriverName,
            School = resource.School,
            District = resource.District,
            Shift = resource.Shift,
            StartTime = resource.StartTime,
            EstimatedEndTime = resource.EstimatedEndTime,
            Capacity = resource.Capacity
           
        };
        
        await tripRepository.AddAsync(trip);
        
        await unitOfWork.CompleteAsync();

        return StatusCode(201, trip);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        var trips = await tripRepository.ListAsync();
        return Ok(trips);
    }
}