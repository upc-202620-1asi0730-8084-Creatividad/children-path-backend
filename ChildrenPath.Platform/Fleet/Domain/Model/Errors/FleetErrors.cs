using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Fleet.Domain.Model.Errors;

public class FleetErrors
{
    
    public static readonly Error AlertNotFound = Error.NotFound(
        "Fleet.AlertNotFound", 
        "The specified maintenance alert was not found.");

    public static readonly Error DatabaseError = Error.Unexpected(
        "Fleet.DatabaseError", 
        "A database error occurred while processing the fleet request.");
    
    public static readonly Error VehicleNotFound = Error.NotFound(
        "Fleet.VehicleNotFound", 
        "The specified vehicle was not found.");
}