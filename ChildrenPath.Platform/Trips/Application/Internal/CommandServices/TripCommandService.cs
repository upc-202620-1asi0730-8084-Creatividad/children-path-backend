using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Trips.Application.CommandServices;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Commands;
using ChildrenPath.Platform.Trips.Domain.Model.Entities;
using ChildrenPath.Platform.Trips.Domain.Model.Errors;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Trips.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Trips.Application.Internal.CommandServices;

public class TripCommandService(ITripRepository tripRepository, IUnitOfWork unitOfWork) : ITripCommandService
{
    public async Task<Result<Trip>> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        var trip = new Trip(
            command.Code,
            command.RouteName,
            command.VehiclePlate,
            command.DriverName,
            command.School,
            command.District,
            command.Shift,
            command.StartTime,
            command.EstimatedEndTime,
            command.Capacity);
        try
        {
            await tripRepository.AddAsync(trip, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException)
        {
            return TripErrors.OperationCancelled.ToFailure<Trip>();
        }
        catch (DbUpdateException)
        {
            return TripErrors.DatabaseError.ToFailure<Trip>();
        }
        catch (Exception)
        {
            return TripErrors.InternalServerError.ToFailure<Trip>();
        }
    }

    public async Task<Result<Trip>> Handle(StartTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId, cancellationToken);
        if (trip is null)
            return TripErrors.TripNotFound.ToFailure<Trip>();

        if (trip.Status == TripStatus.Completed)
            return TripErrors.TripAlreadyCompleted.ToFailure<Trip>();
        trip.Start();

        try
        {
            tripRepository.Update(trip);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException) { return TripErrors.OperationCancelled.ToFailure<Trip>(); }
        catch (DbUpdateException) { return TripErrors.DatabaseError.ToFailure<Trip>(); }
        catch (Exception) { return TripErrors.InternalServerError.ToFailure<Trip>(); }
    }
    
    public async Task<Result<Trip>> Handle(CompleteTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId, cancellationToken);
        if (trip is null)
            return TripErrors.TripNotFound.ToFailure<Trip>();

        if (trip.Status == TripStatus.Completed)
            return TripErrors.TripAlreadyCompleted.ToFailure<Trip>();
        trip.Complete();

        try
        {
            tripRepository.Update(trip);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException) { return TripErrors.OperationCancelled.ToFailure<Trip>(); }
        catch (DbUpdateException) { return TripErrors.DatabaseError.ToFailure<Trip>(); }
        catch (Exception) { return TripErrors.InternalServerError.ToFailure<Trip>(); }
    }
    
    
    public async Task<Result<Trip>> Handle(CancelTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId, cancellationToken);
        if (trip is null)
            return TripErrors.TripNotFound.ToFailure<Trip>();

        if (trip.Status == TripStatus.Cancelled)
            return TripErrors.TripAlreadyCancelled.ToFailure<Trip>();
        trip.Cancel();

        try
        {
            tripRepository.Update(trip);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException) { return TripErrors.OperationCancelled.ToFailure<Trip>(); }
        catch (DbUpdateException) { return TripErrors.DatabaseError.ToFailure<Trip>(); }
        catch (Exception) { return TripErrors.InternalServerError.ToFailure<Trip>(); }
    }

    public async Task<Result<Trip>> Handle(MarkTripAsDelayedCommand command, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId, cancellationToken);
        if (trip is null)
            return TripErrors.TripNotFound.ToFailure<Trip>();
        trip.MarkDelayed();

        try
        {
            tripRepository.Update(trip);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException) { return TripErrors.OperationCancelled.ToFailure<Trip>(); }
        catch (DbUpdateException) { return TripErrors.DatabaseError.ToFailure<Trip>(); }
        catch (Exception) { return TripErrors.InternalServerError.ToFailure<Trip>(); }
    }
    
    
    public async Task<Result<Trip>> Handle(AddRouteStopCommand command, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId, cancellationToken);
        if (trip is null)
            return TripErrors.TripNotFound.ToFailure<Trip>();

        var stop = new RouteStop(command.Name, command.Address, command.Order, command.ScheduledTime);
        trip.AddStop(stop);

        try
        {
            tripRepository.Update(trip);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Trip>.Success(trip);
        }
        catch (OperationCanceledException) { return TripErrors.OperationCancelled.ToFailure<Trip>(); }
        catch (DbUpdateException) { return TripErrors.DatabaseError.ToFailure<Trip>(); }
        catch (Exception) { return TripErrors.InternalServerError.ToFailure<Trip>(); }
    }
} 