using FluentValidation.Results;

namespace VerticalSliceExampel.CommonModule.Validation;

public class ValidationFailed
{
    public IEnumerable<ValidationFailure> Errors { get; }

    public ValidationFailed(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors;
    }
}
