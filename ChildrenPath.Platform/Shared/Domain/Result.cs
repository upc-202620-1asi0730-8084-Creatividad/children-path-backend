namespace ChildrenPath.Platform.Shared.Domain;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Message { get; }

    protected Result(bool isSuccess, T? value, string message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static Result<T> Failure(string message) => new(false, default, message);
}