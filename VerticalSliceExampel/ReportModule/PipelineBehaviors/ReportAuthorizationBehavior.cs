using MediatR;
using VerticalSliceExample.CommonModule;

namespace VerticalSliceExample.ReportModule.PipelineBehaviors;

public class ReportAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, IResponse>
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportAuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<IResponse> next,
        CancellationToken cancellationToken
        )
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token) || token == "invalid")
        {
            return Response<TResponse>.Unauthorized("Unauthorized");
        }

        return await next();
    }
}
