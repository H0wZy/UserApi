namespace user_api.cs.Shared;

public class Result<T>
{
    public bool Success { get; }
    public string? Error { get; }
    public T? Data { get; }

    private Result(bool success, string? error, T? data)
    {
        Success = success;
        Error = error;
        Data = data;
    }

    public static Result<T> Ok(T data) => new(true, null, data);
    public static Result<T> Fail(string error) => new(false, error, default);
}
