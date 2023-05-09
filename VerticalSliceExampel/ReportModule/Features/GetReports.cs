using AutoMapper;
using FluentValidation.Results;
using MediatR;
using VerticalSliceExampel.CommonModule;
using VerticalSliceExampel.CommonModule.Validation;
using VerticalSliceExampel.ReportModule.Models.ViewModels;
using Db = VerticalSliceExampel.ReportModule.Models.Models;

using VerticalSliceExampel.ReportModule.Repositories.Interface;
using System.Linq.Expressions;

namespace VerticalSliceExampel.ReportModule.Features;

public class GetReports : IRequest<IResponse>
{
    public List<string> Descriptions { get; set; }
    public List<Guid> Ids { get; set; }
    public class Handler : IRequestHandler<GetReports, IResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        public Handler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }
        public async Task<IResponse> Handle(GetReports query, CancellationToken cancellationToken)
        {
            Expression<Func<Db.Report, bool>> filter = null;
            if (query?.Ids.Count > 0 && query?.Descriptions.Count > 0)
            {
                filter = x => query.Descriptions.Contains(x.Description) && query.Ids.Contains(x.Id);
            }
            else if (query?.Ids.Count > 0)
            {
                filter = x => query.Ids.Contains(x.Id);
            }
            else if (query?.Descriptions.Count > 0)
            {
                filter = x => query.Descriptions.Contains(x.Description);
            }
            var reports = await _reportRepository.GetAllAsync(filter);

            if (reports?.Any() != true)
            {
                return Response<ReportViewModel>.Ok(new ReportViewModel { ErrorMessage = "Reports not found" });
            }

            var viewModel = new ReportViewModel()
            {
                Reports = _mapper.Map<List<Report>>(reports),
                ErrorMessage = string.Empty
            };

            return Response<ReportViewModel>.Ok(viewModel);
        }
    }
}
