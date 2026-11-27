using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Subscriptions.Application.CommandServices;
using ChildrenPath.Platform.Subscriptions.Application.QueryServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;
using ChildrenPath.Platform.Subscriptions.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Rest;

/// <summary>
///     REST API controller for managing commercial plans, subscriptions, invoices,
///     saved payment methods, and frontend billing dashboard endpoints.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController(
    IPlanQueryService planQueryService,
    IPlanCommandService planCommandService,
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService)
    : ControllerBase
{
    // =========================================================================
    // PLANS ENDPOINTS
    // =========================================================================

    /// <summary>
    ///     Retrieves all subscription plans (optionally filtered to active plans only).
    /// </summary>
    [HttpGet("plans")]
    [EndpointSummary("Get all subscription plans")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlanResource>))]
    public async Task<IActionResult> GetAllPlans(
        [FromQuery] bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        var plans = activeOnly
            ? await planQueryService.Handle(new GetActivePlansQuery(), cancellationToken)
            : await planQueryService.Handle(new GetAllPlansQuery(), cancellationToken);

        var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a single subscription plan by its numeric identifier.
    /// </summary>
    [HttpGet("plans/{planId:int}")]
    [EndpointSummary("Get a subscription plan by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanById([FromRoute] int planId, CancellationToken cancellationToken)
    {
        var plan = await planQueryService.Handle(new GetPlanByIdQuery(planId), cancellationToken);
        if (plan is null) return NotFound();

        return Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    /// <summary>
    ///     Retrieves a single subscription plan by its unique code (e.g., starter, pro, enterprise).
    /// </summary>
    [HttpGet("plans/code/{code}")]
    [EndpointSummary("Get a subscription plan by code")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanByCode([FromRoute] string code, CancellationToken cancellationToken)
    {
        var plan = await planQueryService.Handle(new GetPlanByCodeQuery(code), cancellationToken);
        if (plan is null) return NotFound();

        return Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    /// <summary>
    ///     Creates a new subscription plan.
    /// </summary>
    [HttpPost("plans")]
    [EndpointSummary("Create a new subscription plan")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePlan(
        [FromBody] CreatePlanResource resource,
        CancellationToken cancellationToken)
    {
        var command = PlanCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await planCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => CreatedAtAction(
                nameof(GetPlanById),
                new { planId = created.Id },
                PlanResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    /// <summary>
    ///     Updates an existing subscription plan.
    /// </summary>
    [HttpPut("plans/{planId:int}")]
    [EndpointSummary("Update a subscription plan")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePlan(
        [FromRoute] int planId,
        [FromBody] UpdatePlanResource resource,
        CancellationToken cancellationToken)
    {
        var command = PlanCommandFromResourceAssembler.ToCommandFromResource(planId, resource);
        var result = await planCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Activates or deactivates an existing subscription plan.
    /// </summary>
    [HttpPatch("plans/{planId:int}/status")]
    [EndpointSummary("Toggle a subscription plan active status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TogglePlanStatus(
        [FromRoute] int planId,
        [FromBody] TogglePlanStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = PlanCommandFromResourceAssembler.ToCommandFromResource(planId, resource);
        var result = await planCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    // =========================================================================
    // SUBSCRIPTIONS ENDPOINTS
    // =========================================================================

    /// <summary>
    ///     Retrieves all subscriptions.
    /// </summary>
    [HttpGet]
    [EndpointSummary("Get all subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    public async Task<IActionResult> GetAllSubscriptions(CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionQueryService.Handle(new GetAllSubscriptionsQuery(), cancellationToken);
        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves a single subscription by its numeric identifier.
    /// </summary>
    [HttpGet("{id:int}")]
    [EndpointSummary("Get a subscription by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(id), cancellationToken);
        if (subscription is null) return NotFound();

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    /// <summary>
    ///     Retrieves all subscriptions belonging to a specific user.
    /// </summary>
    [HttpGet("user/{userId:int}")]
    [EndpointSummary("Get subscriptions by user id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    public async Task<IActionResult> GetSubscriptionsByUserId(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionQueryService.Handle(
            new GetSubscriptionsByUserIdQuery(userId),
            cancellationToken);

        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves the active subscription belonging to a specific user.
    /// </summary>
    [HttpGet("user/{userId:int}/active")]
    [EndpointSummary("Get active subscription by user id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveSubscriptionByUserId(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(
            new GetActiveSubscriptionByUserIdQuery(userId),
            cancellationToken);

        if (subscription is null) return NotFound();

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    /// <summary>
    ///     Retrieves all subscriptions matching a specific status (active, pending, past-due, cancelled, expired).
    /// </summary>
    [HttpGet("status/{status}")]
    [EndpointSummary("Get subscriptions by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubscriptionsByStatus(
        [FromRoute] string status,
        CancellationToken cancellationToken)
    {
        var parsedStatus = SubscriptionEnumMapper.ParseSubscriptionStatus(status);
        if (!Enum.IsDefined(parsedStatus))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Subscriptions.InvalidStatus",
                detail: $"The subscription status '{status}' is not valid.");
        }

        var subscriptions = await subscriptionQueryService.Handle(
            new GetSubscriptionsByStatusQuery(parsedStatus),
            cancellationToken);

        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Creates a new subscription and issues its initial invoice and payment.
    /// </summary>
    [HttpPost]
    [EndpointSummary("Create a new subscription")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => CreatedAtAction(
                nameof(GetSubscriptionById),
                new { id = created.Id },
                SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    /// <summary>
    ///     Renews an existing subscription for additional months and records the renewal invoice and payment.
    /// </summary>
    [HttpPost("{id:int}/renew")]
    [EndpointSummary("Renew a subscription")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RenewSubscription(
        [FromRoute] int id,
        [FromBody] RenewSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Switches an existing subscription to another active plan.
    /// </summary>
    [HttpPut("{id:int}/plan")]
    [EndpointSummary("Change subscription plan")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeSubscriptionPlan(
        [FromRoute] int id,
        [FromBody] ChangeSubscriptionPlanResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Cancels an active subscription.
    /// </summary>
    [HttpPatch("{id:int}/cancel")]
    [EndpointSummary("Cancel a subscription")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelSubscription([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(new CancelSubscriptionCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Updates the current vehicle, student, and user usage counters for a subscription.
    /// </summary>
    [HttpPatch("{id:int}/usage")]
    [EndpointSummary("Update subscription resource usage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubscriptionUsage(
        [FromRoute] int id,
        [FromBody] UpdateSubscriptionUsageResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    // =========================================================================
    // INVOICES & SAVED PAYMENT METHODS ENDPOINTS
    // =========================================================================

    /// <summary>
    ///     Registers payment for an unpaid invoice under a subscription.
    /// </summary>
    [HttpPost("{id:int}/invoices/{invoiceId:int}/pay")]
    [EndpointSummary("Pay a subscription invoice")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PayInvoice(
        [FromRoute] int id,
        [FromRoute] int invoiceId,
        [FromBody] PayInvoiceResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, invoiceId, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            paid => Ok(SubscriptionResourceFromEntityAssembler.ToInvoiceResourceFromEntity(paid)));
    }

    /// <summary>
    ///     Attaches a new saved payment card to a subscription.
    /// </summary>
    [HttpPost("{id:int}/payment-methods")]
    [EndpointSummary("Add a saved payment method to a subscription")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SavedPaymentMethodResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddPaymentMethod(
        [FromRoute] int id,
        [FromBody] AddPaymentMethodResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            created => Created(
                $"/api/v1/subscriptions/{id}/payment-methods/{created.Id}",
                SubscriptionResourceFromEntityAssembler.ToPaymentMethodResourceFromEntity(created)));
    }

    /// <summary>
    ///     Marks an existing saved payment card as the primary billing method.
    /// </summary>
    [HttpPatch("{id:int}/payment-methods/{paymentMethodId:int}/primary")]
    [EndpointSummary("Set primary payment method")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPrimaryPaymentMethod(
        [FromRoute] int id,
        [FromRoute] int paymentMethodId,
        CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(
            new SetPrimaryPaymentMethodCommand(id, paymentMethodId),
            cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    /// <summary>
    ///     Removes a saved payment card from a subscription.
    /// </summary>
    [HttpDelete("{id:int}/payment-methods/{paymentMethodId:int}")]
    [EndpointSummary("Remove a saved payment method")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePaymentMethod(
        [FromRoute] int id,
        [FromRoute] int paymentMethodId,
        CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(
            new RemovePaymentMethodCommand(id, paymentMethodId),
            cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }

    // =========================================================================
    // FRONTEND DASHBOARD ENDPOINTS (Vue .env)
    // =========================================================================

    /// <summary>
    ///     Retrieves the current active subscription formatted for the Vue billing dashboard.
    /// </summary>
    [HttpGet("/api/v1/currentSubscription")]
    [EndpointSummary("Get Current Subscription for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentSubscriptionDashboardResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentSubscriptionForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return NotFound();

        return Ok(SubscriptionDashboardAssembler.ToCurrentSubscriptionResource(current));
    }

    /// <summary>
    ///     Retrieves active subscription plans formatted for the Vue plan cards grid.
    /// </summary>
    [HttpGet("/api/v1/subscriptionPlans")]
    [EndpointSummary("Get Subscription Plans for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionPlanDashboardResource>))]
    public async Task<IActionResult> GetSubscriptionPlansForFrontend(CancellationToken cancellationToken)
    {
        var plans = await planQueryService.Handle(new GetActivePlansQuery(), cancellationToken);
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);

        var resources = SubscriptionDashboardAssembler.ToPlanDashboardResources(plans, current?.PlanId);
        return Ok(resources);
    }

    /// <summary>
    ///     Retrieves saved payment methods for the current subscription formatted for the Vue frontend.
    /// </summary>
    [HttpGet("/api/v1/paymentMethods")]
    [EndpointSummary("Get Payment Methods for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentMethodDashboardResource>))]
    public async Task<IActionResult> GetPaymentMethodsForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return Ok(Array.Empty<PaymentMethodDashboardResource>());

        return Ok(SubscriptionDashboardAssembler.ToPaymentMethodDashboardResources(current.PaymentMethods));
    }

    /// <summary>
    ///     Retrieves billing history records for the current subscription formatted for the Vue frontend.
    /// </summary>
    [HttpGet("/api/v1/billingHistory")]
    [EndpointSummary("Get Billing History for Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillingRecordDashboardResource>))]
    public async Task<IActionResult> GetBillingHistoryForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return Ok(Array.Empty<BillingRecordDashboardResource>());

        return Ok(SubscriptionDashboardAssembler.ToBillingHistoryResources(current.Invoices));
    }
}