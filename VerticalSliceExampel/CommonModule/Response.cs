using Azure;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceExample.CommonModule.Validation;

namespace VerticalSliceExample.CommonModule;

public class Response<TValue> : IResponse
{
    public bool IsError { get; }
    public bool IsSuccess => !IsError;
    private readonly TValue Value;
    public IActionResult ActionResult { get; }

    public Response(TValue value, bool isError, IActionResult actionResult)
    {
        Value = value;
        IsError = isError;
        ActionResult = actionResult;
    }

    object IResponse.Value => Value;

    public static IResponse BadRequest<TError>(TError error)
    {
        var actionResult = new BadRequestObjectResult(error);
        return new Response<TValue>(default(TValue), true, actionResult);
    }

    public static Response<TValue> Ok(TValue value)
    {
        var actionResult = new OkObjectResult(value);
        return new Response<TValue>(value, false, actionResult);
    }

    public static Response<TValue> Unauthorized<TError>(TError error)
    {
        return new Response<TValue>(default(TValue), true, new UnauthorizedObjectResult(error));
    }

    public static Response<TValue> NotFound()
    {
        return new Response<TValue>(default(TValue), true, new NotFoundResult());
    }
}
