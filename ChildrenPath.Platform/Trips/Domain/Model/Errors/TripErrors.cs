using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Trips.Domain.Model.Errors;

public static class TripErrors
{
    public static readonly Error TripCreationFailed =
        new("Trips.TripCreationFailed", "An error occurred while creating the trip.");

    public static readonly Error TripNotFound =
        new("Trips.TripNotFound", "The specified trip was not found.");

    public static readonly Error TripAlreadyCompleted =
        new("Trips.TripAlreadyCompleted", "The trip has already been completed.");

    public static readonly Error TripAlreadyCancelled =
        new("Trips.TripAlreadyCancelled", "The trip has already been cancelled.");

    public static readonly Error OperationCancelled =
        new("Trips.OperationCancelled", "The operation was cancelled.");

    public static readonly Error DatabaseError =
        new("Trips.DatabaseError", "A database error occurred while processing the request.");

    public static readonly Error InternalServerError =
        new("Trips.InternalServerError", "An internal server error occurred.");
}