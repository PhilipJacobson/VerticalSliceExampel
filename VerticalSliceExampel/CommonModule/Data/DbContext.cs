using VerticalSliceExample.ReportModule.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace VerticalSliceExample.CommonModule.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }
   
}
