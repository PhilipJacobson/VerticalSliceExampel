namespace VerticalSliceExampel.ReportModule.MappingProfile;

using AutoMapper;
using Db = VerticalSliceExampel.ReportModule.Models.Models;
using VerticalSliceExampel.ReportModule.Models.ViewModels;
using VerticalSliceExampel.ReportModule.Features;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Db.Report, Report>();
        CreateMap<CreateReport, Db.Report>();
    }
}
