using System.Net.Mime;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Subscriptions.Application.CommandServices;
using ChildrenPath.Platform.Subscriptions.Application.QueryServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;
using ChildrenPath.Platform.Subscriptions.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Subscriptions")]
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

    [HttpGet("plans")]
    [EndpointSummary("get all subscription plans")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlanResource>))]
    public async Task<IActionResult> GetAllPlans(
        [FromQuery] bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        var plans = activeOnly
            ? await planQueryService.Handle(new GetActivePlansQuery(), cancellationToken)
            : await planQueryService.Handle(new GetAllPlansQuery(), cancellationToken);

        return Ok(plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("plans/{planId:int}")]
    [EndpointSummary("get a subscription plan by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanById([FromRoute] int planId, CancellationToken cancellationToken)
    {
        var plan = await planQueryService.Handle(new GetPlanByIdQuery(planId), cancellationToken);
        if (plan is null) return NotFound();

        return Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    [HttpGet("plans/code/{code}")]
    [EndpointSummary("get a subscription plan by code")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanByCode([FromRoute] string code, CancellationToken cancellationToken)
    {
        var plan = await planQueryService.Handle(new GetPlanByCodeQuery(code), cancellationToken);
        if (plan is null) return NotFound();

        return Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    [HttpPost("plans")]
    [EndpointSummary("create a new subscription plan")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlanResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePlan([FromBody] CreatePlanResource resource, CancellationToken cancellationToken)
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

    [HttpPut("plans/{planId:int}")]
    [EndpointSummary("update a subscription plan")]
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

    [HttpPatch("plans/{planId:int}/status")]
    [EndpointSummary("toggle a subscription plan active status")]
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

    [HttpGet]
    [EndpointSummary("get all subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    public async Task<IActionResult> GetAllSubscriptions(CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionQueryService.Handle(new GetAllSubscriptionsQuery(), cancellationToken);
        return Ok(subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get a subscription by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(id), cancellationToken);
        if (subscription is null) return NotFound();

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpGet("user/{userId:int}")]
    [EndpointSummary("get subscriptions by user id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    public async Task<IActionResult> GetSubscriptionsByUserId([FromRoute] int userId, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionQueryService.Handle(new GetSubscriptionsByUserIdQuery(userId), cancellationToken);
        return Ok(subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("user/{userId:int}/active")]
    [EndpointSummary("get active subscription by user id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveSubscriptionByUserId([FromRoute] int userId, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(new GetActiveSubscriptionByUserIdQuery(userId), cancellationToken);
        if (subscription is null) return NotFound();

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpGet("status/{status}")]
    [EndpointSummary("get subscriptions by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubscriptionsByStatus([FromRoute] string status, CancellationToken cancellationToken)
    {
        var parsedStatus = SubscriptionEnumMapper.ParseSubscriptionStatus(status);
        if (!Enum.IsDefined(parsedStatus))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Subscriptions.InvalidStatus",
                detail: $"The subscription status '{status}' is not valid.");
        }

        var subscriptions = await subscriptionQueryService.Handle(new GetSubscriptionsByStatusQuery(parsedStatus), cancellationToken);
        return Ok(subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("create a new subscription")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource, CancellationToken cancellationToken)
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

    [HttpPost("{id:int}/renew")]
    [EndpointSummary("renew a subscription")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RenewSubscription([FromRoute] int id, [FromBody] RenewSubscriptionResource resource, CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPut("{id:int}/plan")]
    [EndpointSummary("change subscription plan")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeSubscriptionPlan([FromRoute] int id, [FromBody] ChangeSubscriptionPlanResource resource, CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPatch("{id:int}/cancel")]
    [EndpointSummary("cancel a subscription")]
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

    [HttpPatch("{id:int}/usage")]
    [EndpointSummary("update subscription resource usage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubscriptionUsage([FromRoute] int id, [FromBody] UpdateSubscriptionUsageResource resource, CancellationToken cancellationToken)
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

    [HttpPost("{id:int}/invoices/{invoiceId:int}/pay")]
    [EndpointSummary("pay a subscription invoice")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PayInvoice([FromRoute] int id, [FromRoute] int invoiceId, [FromBody] PayInvoiceResource resource, CancellationToken cancellationToken)
    {
        var command = SubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, invoiceId, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            paid => Ok(SubscriptionResourceFromEntityAssembler.ToInvoiceResourceFromEntity(paid)));
    }

    [HttpPost("{id:int}/payment-methods")]
    [EndpointSummary("add a saved payment method to a subscription")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SavedPaymentMethodResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddPaymentMethod([FromRoute] int id, [FromBody] AddPaymentMethodResource resource, CancellationToken cancellationToken)
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

    [HttpPatch("{id:int}/payment-methods/{paymentMethodId:int}/primary")]
    [EndpointSummary("set primary payment method")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriptionResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPrimaryPaymentMethod([FromRoute] int id, [FromRoute] int paymentMethodId, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(new SetPrimaryPaymentMethodCommand(id, paymentMethodId), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{id:int}/payment-methods/{paymentMethodId:int}")]
    [EndpointSummary("remove a saved payment method")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePaymentMethod([FromRoute] int id, [FromRoute] int paymentMethodId, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(new RemovePaymentMethodCommand(id, paymentMethodId), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            _ => NoContent());
    }

    // =========================================================================
    // FRONTEND DASHBOARD ENDPOINTS
    // =========================================================================

    [HttpGet("current")]
    [EndpointSummary("get current subscription for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentSubscriptionDashboardResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentSubscriptionForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return NotFound();

        return Ok(SubscriptionDashboardAssembler.ToCurrentSubscriptionResource(current));
    }

    [HttpGet("plans/dashboard")]
    [EndpointSummary("get subscription plans for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionPlanDashboardResource>))]
    public async Task<IActionResult> GetSubscriptionPlansForFrontend(CancellationToken cancellationToken)
    {
        var plans = await planQueryService.Handle(new GetActivePlansQuery(), cancellationToken);
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);

        var resources = SubscriptionDashboardAssembler.ToPlanDashboardResources(plans, current?.PlanId);
        return Ok(resources);
    }

    [HttpGet("payment-methods")]
    [EndpointSummary("get payment methods for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentMethodDashboardResource>))]
    public async Task<IActionResult> GetPaymentMethodsForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return Ok(Array.Empty<PaymentMethodDashboardResource>());

        return Ok(SubscriptionDashboardAssembler.ToPaymentMethodDashboardResources(current.PaymentMethods));
    }

    [HttpGet("billing-history")]
    [EndpointSummary("get billing history for frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillingRecordDashboardResource>))]
    public async Task<IActionResult> GetBillingHistoryForFrontend(CancellationToken cancellationToken)
    {
        var current = await subscriptionQueryService.Handle(new GetCurrentSubscriptionQuery(), cancellationToken);
        if (current is null) return Ok(Array.Empty<BillingRecordDashboardResource>());

        return Ok(SubscriptionDashboardAssembler.ToBillingHistoryResources(current.Invoices));
    }
}