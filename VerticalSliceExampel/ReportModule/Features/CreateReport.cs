﻿using AutoMapper;
using MediatR;
using VerticalSliceExample.CommonModule;
using Db = VerticalSliceExample.ReportModule.Models.Models;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using VerticalSliceExample.ReportModule.Repositories.Interface;
using FluentValidation;

namespace VerticalSliceExample.ReportModule.Features;

public class CreateReport : IRequest<IResponse<Report>>
{
        public string Name { get; set; }
        public string Description { get; set; }

    public class Handler : IRequestHandler<CreateReport, IResponse<Report>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public Handler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }
        public async Task<IResponse<Report>> Handle(CreateReport command, CancellationToken cancellationToken)
        {
            var report = _mapper.Map<Db.Report>(command);
            report = await _reportRepository.AddAsync(report);
            var viewReport = _mapper.Map<Report>(report);
            return Response<Report>.Ok(viewReport);
        }
    }
}

public class CreateReportValidator : AbstractValidator<CreateReport>
{
    public CreateReportValidator(IReportRepository reportRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.")
            .MustAsync(async (name, cancellationToken) =>
            {
                var existingReport = await reportRepository.GetByNameAsync(name);
                return existingReport == null;
            }).WithMessage("Name already exists.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(1, 500).WithMessage("Description must be between 1 and 500 characters.");
    }
}
