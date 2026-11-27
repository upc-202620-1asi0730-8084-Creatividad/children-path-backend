using System.Text.Json;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Assembles Plan domain commands from REST resource payloads.
/// </summary>
public static class PlanCommandFromResourceAssembler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static CreatePlanCommand ToCommandFromResource(CreatePlanResource resource)
    {
        var featuresJson = resource.Features is null
            ? "[]"
            : JsonSerializer.Serialize(resource.Features, JsonOptions);

        return new CreatePlanCommand(
            resource.Code,
            resource.Name,
            resource.Price,
            SubscriptionEnumMapper.ParsePlanType(resource.Type),
            resource.VehicleLimit,
            resource.StudentLimit,
            resource.UserLimit,
            resource.Currency,
            resource.Interval,
            resource.IsFeatured,
            featuresJson);
    }

    public static UpdatePlanCommand ToCommandFromResource(int planId, UpdatePlanResource resource)
    {
        var featuresJson = resource.Features is null
            ? null
            : JsonSerializer.Serialize(resource.Features, JsonOptions);

        return new UpdatePlanCommand(
            planId,
            resource.Name,
            resource.Price,
            SubscriptionEnumMapper.ParsePlanType(resource.Type),
            resource.VehicleLimit,
            resource.StudentLimit,
            resource.UserLimit,
            resource.IsFeatured,
            featuresJson);
    }

    public static TogglePlanStatusCommand ToCommandFromResource(int planId, TogglePlanStatusResource resource)
    {
        return new TogglePlanStatusCommand(planId, resource.IsActive);
    }
}