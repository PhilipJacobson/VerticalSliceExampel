using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VerticalSliceExample.CommonModule.Data;
using VerticalSliceExample.ReportModule.Controllers;
using VerticalSliceExample.ReportModule.Repositories.Interface;
using VerticalSliceExample.ReportModule.Repositories;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using Db = VerticalSliceExample.ReportModule.Models.Models;


namespace VerticalSliceExample.Tests.ReportModuleTests;

public class TrationalReportTest
{

    [Fact]
    public async Task GetByIdAsync_ShouldReturnReport_WhenReportExists()
    {
        var reportId = Guid.NewGuid();
        var mockDbSet = new Mock<DbSet<Db.Report>>();
        mockDbSet.Setup(db => db.FindAsync(reportId)).ReturnsAsync(new Db.Report { Id = reportId });

        var mockDbContext = new Mock<AppDbContext>();
        mockDbContext.Setup(ctx => ctx.Reports).Returns(mockDbSet.Object);

        var reportRepository = new ReportRepository(mockDbContext.Object);
        var result = await reportRepository.GetByIdAsync(reportId);

        Assert.NotNull(result);
        Assert.Equal(reportId, result.Id);
    }

    [Fact]
    public async Task GetReportAsync_ShouldReturnReport_WhenReportExists()
    {
        var reportId = Guid.NewGuid();
        var dbReport = new Db.Report { Id = reportId };
        var reportRepositoryMock = new Mock<IReportRepository>();
        reportRepositoryMock.Setup(r => r.GetByIdAsync(reportId)).ReturnsAsync(dbReport);

        var reportService = new ReportService(reportRepositoryMock.Object);
        var result = await reportService.GetReportAsync(reportId);

        Assert.NotNull(result);
        Assert.Equal(reportId, result.Id);
    }

    [Fact]
    public async Task GetReport_ShouldReturnReport_WhenReportExists()
    {
        var reportId = Guid.NewGuid();
        var report = new Report { Id = reportId };
        var reportServiceMock = new Mock<IReportService>();
        reportServiceMock.Setup(s => s.GetReportAsync(reportId)).ReturnsAsync(report);

        var controller = new ReportsControllerApi(reportServiceMock.Object);
        var result = await controller.Get(reportId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReport = Assert.IsType<Report>(okResult.Value);

        Assert.NotNull(returnedReport);
        Assert.Equal(reportId, returnedReport.Id);
    }

}
