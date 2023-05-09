using VerticalSliceExampel.CommonModule.Repository.Interface;
using VerticalSliceExampel.ReportModule.Models.Models;

namespace VerticalSliceExampel.ReportModule.Repositories.Interface;

public interface IReportRepository : IGenericRepository<Report>
{
    public Task<Report> GetByNameAsync(string name);

}
