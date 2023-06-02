using AutoMapper;
using MediatR;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using Db = VerticalSliceExample.ReportModule.Models.Models;

using VerticalSliceExample.ReportModule.Repositories.Interface;
using FluentValidation;

namespace VerticalSliceExample.ReportModule.Features;

public class UpdateReport : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public class Handler : IRequestHandler<UpdateReport, IResponse>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Handler(IReportRepository reportRepository, IMapper mapper, IMediator mediator)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<IResponse> Handle(UpdateReport command, CancellationToken cancellationToken)
        {
            var report = await _reportRepository.GetByIdAsync(command.Id);
            if (report == null)
            {
                return Response<Report>.NotFound();
            }

            UpdateReportFields(command, report);
            await _reportRepository.UpdateAsync(report);

            await _mediator.Publish(
                new PhoneModule.Features.TextMessageSent() { ReportId = report.Id },
                cancellationToken
                ).ConfigureAwait(false);

            var viewReport = _mapper.Map<Report>(report);
            return Response<Report>.Ok(viewReport);
        }

        private void UpdateReportFields(UpdateReport command, Db.Report report)
        {
            if (!string.IsNullOrEmpty(command.Name))
            {
                report.Name = command.Name;
            }
            if (!string.IsNullOrEmpty(command.Description))
            {
                report.Description = command.Description;
            }
        }
        public class UpdateReportValidation : AbstractValidator<UpdateReport>
        {
            public UpdateReportValidation()
            {
                RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ReportId is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid ReportId.");
            }
        }
    }
}
