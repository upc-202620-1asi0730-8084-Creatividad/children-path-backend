using ChildrenPath.Platform.Alerts.Application.CommandServices;
using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.Commands;
using ChildrenPath.Platform.Alerts.Domain.Model.Errors;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects; 
using ChildrenPath.Platform.Alerts.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Alerts.Application.Internal.CommandServices;

public class AlertCommandService(IAlertRepository alertRepository, IUnitOfWork unitOfWork) : IAlertCommandService
{
    public async Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = new Alert(command.TripId, command.Type, command.Priority, command.Message);
        
        try
        {
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return AlertErrors.OperationCancelled.ToFailure<Alert>();
        }
        catch (DbUpdateException)
        {
            return AlertErrors.DatabaseError.ToFailure<Alert>();
        }
        catch (Exception ex)
        {
            return Result<Alert>.Failure($"Error Real en POST: {ex.Message} | Detalle: {ex.InnerException?.Message}");
        }
    }

    public async Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return AlertErrors.AlertNotFound.ToFailure<Alert>();

        if (alert.Status == AlertStatus.Resolved)
            return AlertErrors.AlertAlreadyResolved.ToFailure<Alert>();

        alert.Resolve();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return AlertErrors.OperationCancelled.ToFailure<Alert>();
        }
        catch (DbUpdateException)
        {
            return AlertErrors.DatabaseError.ToFailure<Alert>();
        }
        catch (Exception ex)
        {
            return Result<Alert>.Failure($"Error Real: {ex.Message} | Detalle: {ex.InnerException?.Message}");
        }
    }

    public async Task<Result<Alert>> Handle(CancelAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return AlertErrors.AlertNotFound.ToFailure<Alert>();

        alert.Cancel();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException) { return AlertErrors.OperationCancelled.ToFailure<Alert>(); }
        catch (DbUpdateException) { return AlertErrors.DatabaseError.ToFailure<Alert>(); }
        catch (Exception) { return AlertErrors.InternalServerError.ToFailure<Alert>(); }
    }
}