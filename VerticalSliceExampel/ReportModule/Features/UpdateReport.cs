using AutoMapper;
using MediatR;
using VerticalSliceExampel.CommonModule;
using VerticalSliceExampel.ReportModule.Models.ViewModels;
using VerticalSliceExampel.ReportModule.Repositories;
using VerticalSliceExampel.ReportModule.Repositories.Interface;

namespace VerticalSliceExampel.ReportModule.Features
{
    public class UpdateReport : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public class Handler : IRequestHandler<UpdateReport, IResponse>
        {
            private readonly IReportRepository _reportRepository;
            private readonly IMapper _mapper;

            public Handler(IReportRepository reportRepository, IMapper mapper)
            {
                _reportRepository = reportRepository;
                _mapper = mapper;
            }
            public async Task<IResponse> Handle(UpdateReport command, CancellationToken cancellationToken)
            {
                var report = await _reportRepository.GetByIdAsync(command.Id);
                if (report == null)
                {
                    return Response<Guid>.NotFound();
                }
                report.Name = command.Name;
                report.Description = command.Description;
                await _reportRepository.UpdateAsync(report);
                var viewReport = _mapper.Map<Report>(report);
                return Response<Report>.Ok(viewReport);
            }
        }
    }
}
