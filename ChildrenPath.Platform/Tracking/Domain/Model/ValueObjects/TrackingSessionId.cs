using ChildrenPath.Platform.Shared.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Tracking.Domain.Model.ValueObjects;

public readonly record struct TrackingSessionId(int Value) : IEntityId<int>
{
    public static implicit operator int(TrackingSessionId id) => id.Value;
    public static implicit operator TrackingSessionId(int value) => new(value);
    public override string ToString() => Value.ToString();
}