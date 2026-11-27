using ChildrenPath.Platform.Shared.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Profiles.Domain.Model.ValueObjects;

public readonly record struct ProfileId(int Value) : IEntityId<int>
{
    public static implicit operator int(ProfileId id) => id.Value;
    public static implicit operator ProfileId(int value) => new(value);
    public override string ToString() => Value.ToString();
}