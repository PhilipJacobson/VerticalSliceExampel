using MediatR;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.PhoneModule.Features;
using VerticalSliceExample.PhoneModule.Models.ViewModels;
using VerticalSliceExample.ReportModule.Features;
using VerticalSliceExample.ReportModule.Models.ViewModels;

namespace VerticalSliceExample.Orchestrators;

public class GetPhonesAndCreateReportOrchestrator : IRequest<IResponse>
{
    public class Handler : IRequestHandler<GetPhonesAndCreateReportOrchestrator, IResponse>
    {
        private readonly IMediator _mediator;
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IResponse> Handle(GetPhonesAndCreateReportOrchestrator request, CancellationToken cancellationToken)
        {
            var phoneResponse = await _mediator.Send(new GetPhone(), cancellationToken);
            if (phoneResponse.IsError)
            {
                return Response<Phone>.NotFound();
            }
            var phone = (Phone)phoneResponse.Value;

            var reportsResponse = await _mediator.Send(new GetReports(), cancellationToken);
            if (reportsResponse.IsError)
            {
                return Response<Report>.NotFound();
            }
            var reports = (ReportViewModel)reportsResponse.Value;

            return await _mediator.Send(
                new UpdateReport
                {
                    Description = phone.Number,
                    Id = reports.Reports.First(x => x.Description != phone.Number).Id
                });
        }
    }
}
