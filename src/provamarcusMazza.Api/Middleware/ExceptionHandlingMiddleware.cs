using FluentValidation;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Domain.Common;

namespace provamarcusMazza.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, ex);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, Exception exception)
    {
        var (status, title, errors) = exception switch
        {
            ValidationException validation => (
                StatusCodes.Status400BadRequest,
                "Validation error",
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()) as object),

            DomainException => (
                StatusCodes.Status422UnprocessableEntity,
                "Business rule violation",
                null),

            NotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found",
                null),

            ConflictException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                null),

            UnauthorizedException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                null),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                null)
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new
        {
            status,
            title,
            detail = exception.Message,
            errors
        });
    }
}
