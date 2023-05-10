using VerticalSliceExample.CommonModule.Data;
using VerticalSliceExample.CommonModule.Repository;
using VerticalSliceExample.ReportModule.Models.Models;
using VerticalSliceExample.ReportModule.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace VerticalSliceExample.ReportModule.Repositories;

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
