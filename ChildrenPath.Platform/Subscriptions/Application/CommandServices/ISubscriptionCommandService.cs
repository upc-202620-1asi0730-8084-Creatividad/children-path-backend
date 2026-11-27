using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;

namespace ChildrenPath.Platform.Subscriptions.Application.CommandServices;

/// <summary>
///     Application command service contract for mutating <see cref="Subscription" /> aggregates
///     and their child invoices and payment methods.
/// </summary>
public interface ISubscriptionCommandService
{
    Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(RenewSubscriptionCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(ChangeSubscriptionPlanCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(UpdateSubscriptionUsageCommand command, CancellationToken cancellationToken = default);
    Task<Result<Invoice>> Handle(PayInvoiceCommand command, CancellationToken cancellationToken = default);
    Task<Result<SavedPaymentMethod>> Handle(AddPaymentMethodCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(SetPrimaryPaymentMethodCommand command, CancellationToken cancellationToken = default);
    Task<Result<bool>> Handle(RemovePaymentMethodCommand command, CancellationToken cancellationToken = default);
}