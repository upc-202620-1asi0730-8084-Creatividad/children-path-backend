namespace ChildrenPath.Platform.Shared.Domain.Model;

/// <summary>
///     Represents a structured application/domain error.
/// </summary>
/// <param name="Code">A unique, machine-readable error code.</param>
/// <param name="Message">A human-readable error message.</param>
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}