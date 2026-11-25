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
            return Result<Alert>.Failure(AlertErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Alert>.Failure(AlertErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(AlertErrors.InternalServerError);
        }
    }

    public async Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertErrors.AlertNotFound);

        if (alert.Status == AlertStatus.Resolved)
            return Result<Alert>.Failure(AlertErrors.AlertAlreadyResolved);

        alert.Resolve();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<Alert>.Failure(AlertErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Alert>.Failure(AlertErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(AlertErrors.InternalServerError);
        }
    }

    public async Task<Result<Alert>> Handle(CancelAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertErrors.AlertNotFound);

        alert.Cancel();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException) { return Result<Alert>.Failure(AlertErrors.OperationCancelled); }
        catch (DbUpdateException)          { return Result<Alert>.Failure(AlertErrors.DatabaseError); }
        catch (Exception)                  { return Result<Alert>.Failure(AlertErrors.InternalServerError); }
    }
    
    public async Task<Result<Alert>> Handle(MarkAlertAsReadCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertErrors.AlertNotFound);

        if (alert.IsRead)
            return Result<Alert>.Success(alert); // Si ya está leída, no hacemos nada, devolvemos éxito

        alert.MarkAsRead();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (Exception) // Nota: Aquí te faltaron los catch específicos, te recomiendo agregarlos como en los otros métodos
        {
            return Result<Alert>.Failure(AlertErrors.InternalServerError);
        }
    }
}
