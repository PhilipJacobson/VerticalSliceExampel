using MediatR;
using FluentValidation;
using FluentValidation.Results;
using VerticalSliceExample.CommonModule.Validation;
using Azure;

namespace VerticalSliceExample.CommonModule.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, IResponse>
    where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;
    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<IResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<IResponse> next,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Response<ValidationFailed>.BadRequest(validationResult.Errors);
        }
        return await next();
    }
}
