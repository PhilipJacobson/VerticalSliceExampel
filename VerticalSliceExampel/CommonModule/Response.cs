using Azure;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceExample.CommonModule.Validation;

namespace VerticalSliceExample.CommonModule;

public class Response<TValue> : IResponse<TValue>
{
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private readonly TValue? Value;
    public IActionResult ActionResult { get; }

    private Response(TValue value, bool isError, IActionResult actionResult)
    {
        Value = value;
        IsError = isError;
        ActionResult = actionResult;
    }

    TValue IResponse<TValue>.Value => Value;

    public static Response<TValue> Ok(TValue value)
    {
        var actionResult = new OkObjectResult(value);
        return new Response<TValue>(value, false, actionResult);
    }

    public static Response<TValue> BadRequest<TError>(TError error)
    {
        return new Response<TValue>(default(TValue), true, new BadRequestObjectResult(error));
    }

    public static Response<TValue> NotFound()
    {
        return new Response<TValue>(default(TValue), true, new NotFoundResult());
    }
}
