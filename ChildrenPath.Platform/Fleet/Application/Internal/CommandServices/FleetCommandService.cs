using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Model.Errors;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Application.Internal.CommandServices;

public class FleetCommandService(
    IMaintenanceAlertRepository maintenanceAlertRepository,
    IFleetRepository fleetRepository,
    IUnitOfWork unitOfWork) : IFleetCommandService
{
    public async Task<Result<MaintenanceAlert>> Handle(CreateMaintenanceAlertCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = new MaintenanceAlert(command);
            await maintenanceAlertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<MaintenanceAlert>.Success(alert);
        }
        catch (DbUpdateException)
        {
            return Result<MaintenanceAlert>.Failure(FleetErrors.DatabaseError);
        }
    }

    public async Task<Result> Handle(ResolveMaintenanceAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await maintenanceAlertRepository.FindByIdAsync(command.Id, cancellationToken);
        
        if (alert == null) 
            return Result.Failure(FleetErrors.AlertNotFound);

        try
        {
            alert.Resolve();
            maintenanceAlertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(FleetErrors.DatabaseError);
        }
    }

    public async Task<Result<TransportFleet>> Handle(CreateFleetCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var fleet = new TransportFleet(command);
            await fleetRepository.AddAsync(fleet, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<TransportFleet>.Success(fleet);
        }
        catch (DbUpdateException)
        {
            return Result<TransportFleet>.Failure(FleetErrors.DatabaseError);
        }
    }
}