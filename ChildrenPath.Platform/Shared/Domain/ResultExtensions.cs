using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Shared.Domain;

/// <summary>
///     Extensions to bridge structured <see cref="Error" /> instances with <see cref="Result{T}" />.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts an <see cref="Error" /> into a failed <see cref="Result{T}" />,
    ///     formatting it as "Code: Message".
    /// </summary>
    public static Result<T> ToFailure<T>(this Error error)
        => Result<T>.Failure($"{error.Code}: {error.Message}");

    /// <summary>
    ///     Converts an <see cref="Error" /> into a failed <see cref="Result{T}" />
    ///     using only the message (loses the code).
    /// </summary>
    public static Result<T> ToFailureWithMessage<T>(this Error error)
        => Result<T>.Failure(error.Message);
}