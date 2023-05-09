using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceExampel.CommonModule;

public interface IResponse
{
    bool IsError { get; }
    bool IsSuccess { get; }
    IActionResult ActionResult { get; }
    Object Value { get; }
}
