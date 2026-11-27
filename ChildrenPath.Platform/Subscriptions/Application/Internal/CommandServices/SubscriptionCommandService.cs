using System.Globalization;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Subscriptions.Application.CommandServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Errors;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Subscriptions.Application.Internal.CommandServices;

/// <summary>
///     Default implementation of <see cref="ISubscriptionCommandService" />.
/// </summary>
/// <param name="subscriptionRepository">The repository used to persist subscription aggregates.</param>
/// <param name="planRepository">The repository used to validate subscription plans.</param>
/// <param name="unitOfWork">The unit of work coordinating database transactions.</param>
public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0 || command.PlanId <= 0)
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidSubscriptionData);

        if (command.DurationInMonths <= 0)
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidDuration);

        if (!Enum.IsDefined(command.PaymentMethod))
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidPaymentMethod);

        if (!Enum.IsDefined(command.RenewalMode))
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidRenewalMode);

        var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null)
            return Result<Subscription>.Failure(SubscriptionErrors.PlanNotFound);

        if (!plan.IsActive)
            return Result<Subscription>.Failure(SubscriptionErrors.PlanInactive);

        var hasActive = await subscriptionRepository.ExistsActiveByUserIdAsync(command.UserId, cancellationToken);
        if (hasActive)
            return Result<Subscription>.Failure(SubscriptionErrors.ActiveSubscriptionAlreadyExists);

        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(command.DurationInMonths);

        var subscription = new Subscription(
            command.UserId,
            plan.Id,
            startDate,
            endDate,
            command.ContractOwner,
            command.BillingEmail,
            command.RenewalMode,
            command.SupportContact,
            command.PlanScope,
            command.VehiclesUsed,
            command.StudentsUsed,
            command.UsersUsed);

        var invoiceAmount = (plan.Price ?? 0m) * command.DurationInMonths;
        var invoiceNumber = GenerateInvoiceNumber();
        var description = $"{plan.Name} — {startDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture)}";

        var invoice = subscription.AddInvoice(
            invoiceNumber,
            description,
            invoiceAmount,
            plan.Currency,
            InvoiceStatus.Pending,
            command.PaymentMethod.ToString());

        invoice.RegisterPayment(command.PaymentMethod, methodSummary: command.PaymentMethod.ToString());

        try
        {
            await subscriptionRepository.AddAsync(subscription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var loaded = await subscriptionRepository.FindByIdAsync(subscription.Id, cancellationToken);
            return Result<Subscription>.Success(loaded ?? subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        RenewSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.AdditionalMonths <= 0)
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidDuration);

        if (!Enum.IsDefined(command.PaymentMethod))
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidPaymentMethod);

        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        var baseDate = subscription.EndDate > DateTime.UtcNow ? subscription.EndDate : DateTime.UtcNow;
        var newEndDate = baseDate.AddMonths(command.AdditionalMonths);
        subscription.Renew(newEndDate);

        var planPrice = subscription.Plan?.Price ?? 0m;
        var planCurrency = subscription.Plan?.Currency ?? "USD";
        var planName = subscription.Plan?.Name ?? "Subscription";

        var primaryCard = subscription.PaymentMethods.FirstOrDefault(pm => pm.IsPrimary);
        var methodSummary = primaryCard?.ToDisplaySummary() ?? command.PaymentMethod.ToString();

        var invoice = subscription.AddInvoice(
            GenerateInvoiceNumber(),
            $"{planName} — Renewal ({DateTime.UtcNow.ToString("MMMM yyyy", CultureInfo.InvariantCulture)})",
            planPrice * command.AdditionalMonths,
            planCurrency,
            InvoiceStatus.Pending,
            methodSummary);

        invoice.RegisterPayment(command.PaymentMethod, methodSummary: methodSummary);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            var loaded = await subscriptionRepository.FindByIdAsync(subscription.Id, cancellationToken);
            return Result<Subscription>.Success(loaded ?? subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        ChangeSubscriptionPlanCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.DurationInMonths <= 0)
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidDuration);

        if (!Enum.IsDefined(command.PaymentMethod))
            return Result<Subscription>.Failure(SubscriptionErrors.InvalidPaymentMethod);

        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        var newPlan = await planRepository.FindByIdAsync(command.NewPlanId, cancellationToken);
        if (newPlan is null)
            return Result<Subscription>.Failure(SubscriptionErrors.PlanNotFound);

        if (!newPlan.IsActive)
            return Result<Subscription>.Failure(SubscriptionErrors.PlanInactive);

        var newEndDate = DateTime.UtcNow.AddMonths(command.DurationInMonths);
        subscription.ChangePlan(newPlan.Id, newEndDate);

        var primaryCard = subscription.PaymentMethods.FirstOrDefault(pm => pm.IsPrimary);
        var methodSummary = primaryCard?.ToDisplaySummary() ?? command.PaymentMethod.ToString();

        var invoice = subscription.AddInvoice(
            GenerateInvoiceNumber(),
            $"{newPlan.Name} — Plan Change ({DateTime.UtcNow.ToString("MMMM yyyy", CultureInfo.InvariantCulture)})",
            (newPlan.Price ?? 0m) * command.DurationInMonths,
            newPlan.Currency,
            InvoiceStatus.Pending,
            methodSummary);

        invoice.RegisterPayment(command.PaymentMethod, methodSummary: methodSummary);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            var loaded = await subscriptionRepository.FindByIdAsync(subscription.Id, cancellationToken);
            return Result<Subscription>.Success(loaded ?? subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        CancelSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        if (subscription.Status == SubscriptionStatus.Cancelled)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionAlreadyCancelled);

        subscription.Cancel();

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        UpdateSubscriptionUsageCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        subscription.UpdateUsage(command.VehiclesUsed, command.StudentsUsed, command.UsersUsed);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Invoice>> Handle(
        PayInvoiceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(command.Method))
            return Result<Invoice>.Failure(SubscriptionErrors.InvalidPaymentMethod);

        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Invoice>.Failure(SubscriptionErrors.SubscriptionNotFound);

        var invoice = subscription.Invoices.FirstOrDefault(i => i.Id == command.InvoiceId);
        if (invoice is null)
            return Result<Invoice>.Failure(SubscriptionErrors.InvoiceNotFound);

        if (invoice.Status == InvoiceStatus.Paid)
            return Result<Invoice>.Failure(SubscriptionErrors.InvoiceAlreadyPaid);

        var primaryCard = subscription.PaymentMethods.FirstOrDefault(pm => pm.IsPrimary);
        var summary = !string.IsNullOrWhiteSpace(command.MethodSummary)
            ? command.MethodSummary
            : primaryCard?.ToDisplaySummary() ?? command.Method.ToString();

        invoice.RegisterPayment(command.Method, methodSummary: summary);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Invoice>.Success(invoice);
        }
        catch (OperationCanceledException)
        {
            return Result<Invoice>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Invoice>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Invoice>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<SavedPaymentMethod>> Handle(
        AddPaymentMethodCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(command.Brand))
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.InvalidCardBrand);

        if (string.IsNullOrWhiteSpace(command.Holder) ||
            string.IsNullOrWhiteSpace(command.CardNumberOrLast4) ||
            string.IsNullOrWhiteSpace(command.ExpiresOn))
        {
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.InvalidCardData);
        }

        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.SubscriptionNotFound);

        var paymentMethod = subscription.AddPaymentMethod(
            command.Brand,
            command.CardNumberOrLast4,
            command.Holder,
            command.ExpiresOn,
            command.MakePrimary);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<SavedPaymentMethod>.Success(paymentMethod);
        }
        catch (OperationCanceledException)
        {
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<SavedPaymentMethod>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(
        SetPrimaryPaymentMethodCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        if (!subscription.SetPrimaryPaymentMethod(command.PaymentMethodId))
            return Result<Subscription>.Failure(SubscriptionErrors.PaymentMethodNotFound);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<bool>> Handle(
        RemovePaymentMethodCommand command,
        CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<bool>.Failure(SubscriptionErrors.SubscriptionNotFound);

        if (!subscription.RemovePaymentMethod(command.PaymentMethodId))
            return Result<bool>.Failure(SubscriptionErrors.PaymentMethodNotFound);

        try
        {
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (OperationCanceledException)
        {
            return Result<bool>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<bool>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    private static string GenerateInvoiceNumber()
    {
        return $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..4].ToUpperInvariant()}";
    }
}