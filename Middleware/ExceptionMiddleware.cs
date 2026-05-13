using AutoMapper;
using user_api.cs.Shared;

namespace user_api.cs.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsJsonAsync(GenericResponse<bool>.BadRequest(ex.Message));
        }
    }
}