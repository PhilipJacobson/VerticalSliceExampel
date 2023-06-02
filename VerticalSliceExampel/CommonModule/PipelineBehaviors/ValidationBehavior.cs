using MediatR;
using FluentValidation;
using FluentValidation.Results;

namespace VerticalSliceExample.CommonModule.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResponse
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validator = validators.FirstOrDefault();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        if (_validator != null)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return (TResponse)Response<IEnumerable<ValidationFailure>>.BadRequest(validationResult.Errors);
            }
        }

        return await next();
    }
}
