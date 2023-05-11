using MediatR;
using VerticalSliceExample.ReportModule.Features;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceExample.Orchestrators;

namespace VerticalSliceExample.ReportModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsControllerApi : Controller
{
    private readonly IMediator _mediator;
    public ReportsControllerApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new GetReport { Id = id });
        return response.ActionResult;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateReport command)
    {
        var response = await _mediator.Send(command);
        return response.ActionResult;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromBody] DeleteReport command)
    {
        var response = await _mediator.Send(command);
        return response.ActionResult;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReport command)
    {
        command.Id = id;
        var response = await _mediator.Send(command);
        return response.ActionResult;
    }

    [HttpPost("phone/{phoneId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReportFromPhone(Guid phoneId)
    {
        var response = await _mediator.Send(new GetPhonesAndCreateReportOrchestrator { PhoneId = phoneId });
        return response.ActionResult;
    }
}
