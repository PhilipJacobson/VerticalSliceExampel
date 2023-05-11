using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceExample.CommonModule;

public interface IResponse<T>
{
    bool IsError { get; }
    bool IsSuccess { get; }
    IActionResult ActionResult { get; }
    T Value { get; }
}
