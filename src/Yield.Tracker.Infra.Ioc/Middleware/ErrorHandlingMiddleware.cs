using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Yield.Tracker.Infra.Ioc.Middleware.Exceptions;

namespace Yield.Tracker.Infra.Ioc.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        (int statusCode, object response) = exception switch
        {
            DomainException domainException => (
                MapErrorToStatusCode(domainException.FirstError),
                domainException.Errors
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                new List<Error> { Error.Unexpected(description: "Ocorreu um erro inesperado.") }
            )
        };

        logger.LogError(exception, "Erro tratado no middleware: {Message}", exception.Message);

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static int MapErrorToStatusCode(Error error) =>
        error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
}
