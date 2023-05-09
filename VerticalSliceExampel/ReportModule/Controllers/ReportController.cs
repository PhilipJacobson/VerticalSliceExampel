using MediatR;
using VerticalSliceExampel.ReportModule.Features;
using Microsoft.AspNetCore.Mvc;

namespace VerticalSliceExampel.ReportModule.Controllers;

[Route("reports")]
public class ReportController : Controller
{
    private readonly IMediator _mediator;
    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = new GetReports { Ids = new List<Guid>(), Descriptions = new List<string>() };
        var result = await _mediator.Send(query);
        return View("~/ReportModule/View/Report.cshtml", result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> GetReports(GetReports query)
    {
        var result = await _mediator.Send(query);
        return View("~/ReportModule/View/Report.cshtml", result.Value);
    }
}
