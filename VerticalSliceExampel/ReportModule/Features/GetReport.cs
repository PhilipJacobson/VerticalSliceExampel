using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using VerticalSliceExampel.CommonModule;
using VerticalSliceExampel.CommonModule.Validation;
using VerticalSliceExampel.ReportModule.Models.ViewModels;
using VerticalSliceExampel.ReportModule.Repositories.Interface;

namespace VerticalSliceExampel.ReportModule.Features;

public class GetReport : IRequest<IResponse>
{
    public Guid Id { get; set; }

    public class Handler : IRequestHandler<GetReport, IResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        public Handler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(
            GetReport query,
            CancellationToken cancellationToken
            )
        {
            var report = await _reportRepository.GetByIdAsync(query.Id);
            if (report == null)
            {
                return Response<Report>.NotFound();
            }
            var response = _mapper.Map<Report>(report);
            return Response<Report>.Ok(response);
        }
    }
    public class GetReportValidation : AbstractValidator<GetReport>
    {
        public GetReportValidation()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ReportId is required.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid ReportId.");
        }
    }
}
