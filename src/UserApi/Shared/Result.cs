namespace UserApi.Shared;

public sealed record Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public IReadOnlyCollection<string>? Errors { get; }
    public string? Error => Errors?.FirstOrDefault();

    private Result(T? data)
    {
        IsSuccess = true;
        Data = data;
    }

    private Result(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors.ToList();
    }

    public bool IsFailure => !IsSuccess;

    public static Result<T> Ok(T data) => new(data);
    public static Result<T> Fail(string error) => new([error]);
    public static Result<T> Fail(IEnumerable<string> errors) => new(errors);
}
