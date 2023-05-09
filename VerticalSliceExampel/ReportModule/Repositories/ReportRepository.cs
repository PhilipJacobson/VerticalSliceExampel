using VerticalSliceExampel.CommonModule.Data;
using VerticalSliceExampel.CommonModule.Repository;
using VerticalSliceExampel.ReportModule.Models.Models;
using VerticalSliceExampel.ReportModule.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace VerticalSliceExampel.ReportModule.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly AppDbContext _context;
    public ReportRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Report> GetByNameAsync(string name)
    {
        return await _context.Reports.FirstOrDefaultAsync(x => x.Name == name);
    }
}
