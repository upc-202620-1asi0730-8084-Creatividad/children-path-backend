namespace ChildrenPath.Platform.Shared.Domain.Model.Entities;

/// <summary>
///     Marks an entity with creation/update timestamps.
/// </summary>
public interface IAuditableEntity
{
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}