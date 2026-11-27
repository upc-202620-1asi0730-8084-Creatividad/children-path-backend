using System.Text.Json;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="PlanResource" /> DTOs from <see cref="Plan" /> aggregates.
/// </summary>
public static class PlanResourceFromEntityAssembler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static PlanResource ToResourceFromEntity(Plan entity)
    {
        return new PlanResource(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.Price,
            entity.Currency,
            entity.Interval,
            entity.Type.ToString(),
            entity.VehicleLimit,
            entity.StudentLimit,
            entity.UserLimit,
            entity.IsFeatured,
            entity.IsActive,
            DeserializeFeatures(entity.FeaturesJson));
    }

    public static IReadOnlyList<PlanFeatureResource> DeserializeFeatures(string? featuresJson)
    {
        if (string.IsNullOrWhiteSpace(featuresJson))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<PlanFeatureResource>>(featuresJson, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}