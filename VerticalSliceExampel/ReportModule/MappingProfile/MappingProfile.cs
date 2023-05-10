namespace VerticalSliceExample.ReportModule.MappingProfile;

using AutoMapper;
using Db = VerticalSliceExample.ReportModule.Models.Models;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using VerticalSliceExample.ReportModule.Features;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Db.Report, Report>();
        CreateMap<CreateReport, Db.Report>();
    }
}
