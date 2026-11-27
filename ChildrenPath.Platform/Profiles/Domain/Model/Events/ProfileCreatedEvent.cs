using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Profiles.Domain.Model.Events;

public class ProfileCreatedEvent(int userId, string firstName, string lastName, UserRole role) : IEvent
{
    public int UserId { get; } = userId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public UserRole Role { get; } = role;
}