using AutoMapper;
using Moq;
using VerticalSliceExample.ReportModule.Features;
using VerticalSliceExample.ReportModule.Repositories.Interface;
using Db = VerticalSliceExample.ReportModule.Models.Models;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class GetReportTests
{
    [Fact]
    public async Task Handler_ShouldReturnReport_WhenReportExists()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var report = new Db.Report { Id = reportId };
        var reportViewModel = new Report { Id = reportId };

        var reportRepositoryMock = new Mock<IReportRepository>();
        reportRepositoryMock.Setup(r => r.GetByIdAsync(reportId)).ReturnsAsync(report);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Report>(report)).Returns(reportViewModel);

        var handler = new GetReport.Handler(reportRepositoryMock.Object, mapperMock.Object);
        var query = new GetReport { Id = reportId };

        var response = await handler.Handle(query, CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(reportViewModel, response.Value);
    }

    [Fact]
    public async Task Handler_ShouldReturnNotFound_WhenReportDoesNotExist()
    {
        var reportId = Guid.NewGuid();
        var reportRepositoryMock = new Mock<IReportRepository>();
        reportRepositoryMock.Setup(r => r.GetByIdAsync(reportId)).ReturnsAsync((Db.Report)null);

        var mapperMock = new Mock<IMapper>();
        var handler = new GetReport.Handler(reportRepositoryMock.Object, mapperMock.Object);
        var query = new GetReport { Id = reportId };

        var response = await handler.Handle(query, CancellationToken.None);

        Assert.True(response.ActionResult is NotFoundResult);
    }

    [Fact]
    public async Task Validator_ShouldHaveValidationError_WhenIdIsEmpty()
    {
        var validator = new GetReport.GetReportValidation();
        var query = new GetReport { Id = Guid.Empty };

        var result = await validator.ValidateAsync(query);

        Assert.Contains(result.Errors, e => e.PropertyName == "Id" && e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]
    public async Task Validator_ShouldNotHaveValidationError_WhenIdIsValid()
    {
        var validator = new GetReport.GetReportValidation();
        var query = new GetReport { Id = Guid.NewGuid() };

        var result = await validator.ValidateAsync(query);

        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Id" && e.ErrorCode == "NotEmptyValidator");
    }
}
