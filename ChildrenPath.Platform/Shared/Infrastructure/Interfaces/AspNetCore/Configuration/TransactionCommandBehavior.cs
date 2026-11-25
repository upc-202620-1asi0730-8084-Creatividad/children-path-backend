using ChildrenPath.Platform.Shared.Domain.Repositories;
using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;

namespace ChildrenPath.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;

public class TransactionCommandBehavior<TCommand>(IUnitOfWork uow, ILogger<TransactionCommandBehavior<TCommand>> log)
    : ICommandPipelineBehavior<TCommand> where TCommand : ICommand
{
    public async Task Handle(TCommand cmd, CommandHandlerDelegate next, CancellationToken ct)
    {
        await next();
        log.LogDebug("Committing UoW for {Command}", typeof(TCommand).Name);
        await uow.CompleteAsync(ct);
    }
}