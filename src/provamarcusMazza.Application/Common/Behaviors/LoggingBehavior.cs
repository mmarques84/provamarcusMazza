using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace provamarcusMazza.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation("Handling {RequestName} {@Request}", typeof(TRequest).Name, request);

        try
        {
            var response = await next();
            stopwatch.Stop();
            logger.LogInformation(
                "Handled {RequestName} in {ElapsedMilliseconds}ms {@Response}",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                response);
            return response;
        }
        catch
        {
            stopwatch.Stop();
            logger.LogError(
                "Failed {RequestName} after {ElapsedMilliseconds}ms",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
