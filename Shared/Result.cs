namespace user_api.cs.Shared;

public class Result<T>
{
    public bool Success { get; }
    public string? Error { get; }
    public T? Value { get; }

    private Result(bool success, string? error, T? value)
    {
        Success = success;
        Error = error;
        Value = value;
    }

    public static Result<T> Ok(T value) => new(true, null, value);
    public static Result<T> Fail(string error) => new(false, error, default);
}
