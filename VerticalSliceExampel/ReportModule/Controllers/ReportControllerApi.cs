using MediatR;
using VerticalSliceExample.ReportModule.Features;
using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceExample.ReportModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportControllerAPI : Controller
{
    private readonly IMediator _mediator;
    public ReportControllerAPI(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetReport { Id = id });
        return result.ActionResult;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport(CreateReport command)
    {
        var result = await _mediator.Send(command);
        return result.ActionResult;
    }
}
