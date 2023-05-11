using MediatR;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.PhoneModule.Features;
using VerticalSliceExample.PhoneModule.Models.ViewModels;
using VerticalSliceExample.ReportModule.Features;
using VerticalSliceExample.ReportModule.Models.ViewModels;

namespace VerticalSliceExample.Orchestrators;

public class GetPhonesAndCreateReportOrchestrator : IRequest<IResponse<Report>>
{
    public class Handler : IRequestHandler<GetPhonesAndCreateReportOrchestrator, IResponse<Report>>
    {
        private readonly IMediator _mediator;
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IResponse<Report>> Handle(GetPhonesAndCreateReportOrchestrator request, CancellationToken cancellationToken)
        {
            var phone = await _mediator.Send(new GetPhone(), cancellationToken);
            if (phone.IsError)
            {
                return Response<Report>.NotFound();
            }
            var reports = await _mediator.Send(new GetReports());
            if (reports.IsError)
            {
                return Response<Report>.NotFound();
            }

            return await _mediator.Send(
                new UpdateReport
                {
                    Description = phone.Value.Number,
                    Id = reports.Value.Reports.First(x => x.Description != phone.Value.Number).Id
                });
        }
    }
}
