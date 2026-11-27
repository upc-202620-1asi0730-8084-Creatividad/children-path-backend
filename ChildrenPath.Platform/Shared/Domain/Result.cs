using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Shared.Domain;

public class Result<T>
{
    protected Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? (isSuccess ? Error.None : Error.Failure("Error.Unknown", "Unknown error"));
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public Error Error { get; }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static Result<T> Failure(Error error) => new(false, default, error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
        => IsSuccess ? onSuccess(Value!) : onFailure(Error);
}

public class Result : Result<object>
{
    private Result(bool isSuccess, Error? error) : base(isSuccess, null, error) { }
    public static Result Success() => new(true, Error.None);
    public static new Result Failure(Error error) => new(false, error);
}