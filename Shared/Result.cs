namespace user_api.cs.Shared;

public sealed record Result<T>
{
    private bool IsSuccess { get; }
    public string? Error { get; }
    public T? Data { get; }

    private Result(T? data)
    {
        IsSuccess = true;
        Data = data;
    }

    private Result(string error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsFailure => !IsSuccess;

    public static Result<T> Ok(T data) => new(data);
    public static Result<T> Fail(string error) => new(error);
}
