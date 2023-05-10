using VerticalSliceExample.CommonModule.Repository.Interface;
using VerticalSliceExample.ReportModule.Models.Models;

namespace VerticalSliceExample.ReportModule.Repositories.Interface;

public interface IReportRepository : IGenericRepository<Report>
{
    public Task<Report> GetByNameAsync(string name);

}
