using MediatR;
using FluentValidation;
using FluentValidation.Results;
using VerticalSliceExample.CommonModule.Validation;
using Azure;

namespace VerticalSliceExample.CommonModule.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, IResponse<TResponse>>
    where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;
    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResponse<TResponse>> Handle(
        TRequest request,
        RequestHandlerDelegate<IResponse<TResponse>> next,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Response<TResponse>.BadRequest(validationResult.Errors);
        }
        return await next();
    }
}
