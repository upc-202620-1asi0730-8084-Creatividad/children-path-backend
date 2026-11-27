using ChildrenPath.Platform.Iam.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Iam.Domain.Model.Events;

public record UserRegisteredEvent(UserId UserId, string Username) : IEvent;