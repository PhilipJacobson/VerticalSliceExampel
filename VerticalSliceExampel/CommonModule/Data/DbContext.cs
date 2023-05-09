using VerticalSliceExampel.ReportModule.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace VerticalSliceExampel.CommonModule.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }
   
}
