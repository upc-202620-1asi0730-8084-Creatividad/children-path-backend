using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Errors;

/// <summary>
///     Centralized domain error definitions for the Subscriptions bounded context.
/// </summary>
public static class SubscriptionErrors
{
    // Plan errors
    public static readonly Error PlanNotFound = Error.NotFound(
        "Subscriptions.PlanNotFound",
        "The specified subscription plan was not found.");

    public static readonly Error PlanCodeAlreadyExists = Error.Conflict(
        "Subscriptions.PlanCodeAlreadyExists",
        "A subscription plan with the specified code already exists.");

    public static readonly Error PlanInactive = Error.Validation(
        "Subscriptions.PlanInactive",
        "The selected subscription plan is currently inactive.");

    public static readonly Error InvalidPlanData = Error.Validation(
        "Subscriptions.InvalidPlanData",
        "Plan code and plan name are required.");

    public static readonly Error InvalidPlanType = Error.Validation(
        "Subscriptions.InvalidPlanType",
        "The provided plan type is not valid.");

    // Subscription errors
    public static readonly Error SubscriptionNotFound = Error.NotFound(
        "Subscriptions.NotFound",
        "The specified subscription was not found.");

    public static readonly Error ActiveSubscriptionAlreadyExists = Error.Conflict(
        "Subscriptions.ActiveAlreadyExists",
        "The user already has an active subscription.");

    public static readonly Error SubscriptionAlreadyCancelled = Error.Conflict(
        "Subscriptions.AlreadyCancelled",
        "The subscription is already cancelled.");

    public static readonly Error InvalidSubscriptionData = Error.Validation(
        "Subscriptions.InvalidData",
        "User identifier and plan identifier must be greater than zero.");

    public static readonly Error InvalidDuration = Error.Validation(
        "Subscriptions.InvalidDuration",
        "Subscription duration must be at least 1 month.");

    public static readonly Error InvalidRenewalMode = Error.Validation(
        "Subscriptions.InvalidRenewalMode",
        "The provided renewal mode is not valid.");

    public static readonly Error InvalidSubscriptionStatus = Error.Validation(
        "Subscriptions.InvalidStatus",
        "The provided subscription status is not valid.");

    // Invoice, payment & payment method errors
    public static readonly Error InvoiceNotFound = Error.NotFound(
        "Subscriptions.InvoiceNotFound",
        "The specified invoice was not found.");

    public static readonly Error InvoiceAlreadyPaid = Error.Conflict(
        "Subscriptions.InvoiceAlreadyPaid",
        "The specified invoice has already been paid.");

    public static readonly Error InvalidInvoiceData = Error.Validation(
        "Subscriptions.InvalidInvoiceData",
        "Invoice description is required and amount cannot be negative.");

    public static readonly Error InvalidPaymentMethod = Error.Validation(
        "Subscriptions.InvalidPaymentMethod",
        "The provided payment method is not valid.");

    public static readonly Error PaymentMethodNotFound = Error.NotFound(
        "Subscriptions.PaymentMethodNotFound",
        "The specified saved payment method was not found.");

    public static readonly Error InvalidCardBrand = Error.Validation(
        "Subscriptions.InvalidCardBrand",
        "The provided card brand is not valid (allowed: visa, mastercard, amex).");

    public static readonly Error InvalidCardData = Error.Validation(
        "Subscriptions.InvalidCardData",
        "Card holder, card number/last4, and expiration date are required.");

    // Common infrastructure errors
    public static readonly Error OperationCancelled = Error.Conflict(
        "Subscriptions.OperationCancelled",
        "The operation was cancelled.");

    public static readonly Error DatabaseError = Error.Unexpected(
        "Subscriptions.DatabaseError",
        "A database error occurred while processing the subscription.");

    public static readonly Error InternalServerError = Error.Unexpected(
        "Subscriptions.InternalServerError",
        "An unexpected error occurred while processing the subscription.");
}