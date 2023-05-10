using FluentValidation;
using MediatR;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.ReportModule.Repositories.Interface;

namespace VerticalSliceExample.ReportModule.Features
{
    public class DeleteReport : IRequest<IResponse>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<DeleteReport, IResponse>
        {
            private readonly IReportRepository _reportRepository;
            public Handler(IReportRepository reportRepository)
            {
                _reportRepository = reportRepository;
            }
            public async Task<IResponse> Handle(DeleteReport command, CancellationToken cancellationToken)
            {
                var report = await _reportRepository.GetByIdAsync(command.Id);
                if (report == null)
                {
                    return Response<Guid>.NotFound();
                }
                await _reportRepository.DeleteAsync(report.Id);
                return Response<Guid>.Ok(report.Id);
            }
        }
    }

    public class DeleteReportValidator : AbstractValidator<DeleteReport>
    {
        public DeleteReportValidator()
        {
            RuleFor(x => x.Id)
           .NotEmpty().WithMessage("ReportId is required.")
           .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid ReportId.");
        }
    }
}
