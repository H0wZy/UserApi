using System.Net;

namespace UserApi.Shared;

public record GenericResponse<T>
{
    public bool Success { get; init; }
    public string Message { get; init; }
    public T? Data { get; init; }
    public IEnumerable<string>? Errors { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private GenericResponse(bool success, string message, T? data, HttpStatusCode statusCode, IEnumerable<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
        Errors = errors;
    }

    public static GenericResponse<T> Ok(T data, string message = "Operação realizada com sucesso!") =>
        new(true, message, data, HttpStatusCode.OK);

    public static GenericResponse<T> Created(T data, string message = "Recurso criado com sucesso!") =>
        new(true, message, data, HttpStatusCode.Created);

    public static GenericResponse<T> NotFound(string message = "Recurso não encontrado.") =>
        new(false, message, default, HttpStatusCode.NotFound);

    public static GenericResponse<T> BadRequest(string message, IEnumerable<string>? errors = null) =>
        new(false, message, default, HttpStatusCode.BadRequest, errors);

    // UTILIZADO EM VALIDAÇÕES, EXEMPLO: EMAIL JÁ EXISTE ETC...
    public static GenericResponse<T> Conflict(string message) =>
        new(false, message, default, HttpStatusCode.Conflict);

    public static GenericResponse<T> InternalServerError(string message = "Erro interno do servidor.") =>
        new(false, message, default, HttpStatusCode.InternalServerError);
}