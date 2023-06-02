namespace VerticalSliceExample.CommonModule.PipelineBehaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        _logger.LogInformation($"START {typeof(TRequest).Name}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await next();

        stopwatch.Stop();
        _logger.LogInformation($"END {typeof(TRequest).Name} in {stopwatch.ElapsedMilliseconds}Ms");
        return response;
    }
}
