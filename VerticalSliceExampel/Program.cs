using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.CommonModule.Data;
using VerticalSliceExample.CommonModule.PipelineBehaviors;
using VerticalSliceExample.CommonModule.Repository;
using VerticalSliceExample.CommonModule.Repository.Interface;
using VerticalSliceExample.ReportModule.Features;
using VerticalSliceExample.ReportModule.Models.ViewModels;
using VerticalSliceExample.ReportModule.PipelineBehaviors;
using VerticalSliceExample.ReportModule.Repositories;
using VerticalSliceExample.ReportModule.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<RazorViewEngineOptions>(options =>
    {
        options.AreaViewLocationFormats.Clear();
        options.AreaViewLocationFormats.Add("/CommonModule/Pages/{2}/{1}/{0}.cshtml");
        options.AreaViewLocationFormats.Add("/CommonModule/Pages/Shared/{0}.cshtml");
    });

    services.AddLogging();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddRazorPages();
    services.AddAutoMapper(typeof(Program).Assembly);
    services.AddScoped<IReportRepository, ReportRepository>();
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("MediatRTestConnectionString")));
    services.AddHttpContextAccessor();
    services.AddMediatR(c =>
        c.RegisterServicesFromAssemblyContaining<Program>()
            .AddBehavior<IPipelineBehavior<DeleteReport, IResponse>, ReportAuthorizationBehavior<DeleteReport, IResponse>>()
            .AddOpenBehavior(typeof(ValidationBehavior<,>))
            .AddOpenBehavior(typeof(LoggingBehavior<,>)));
    services.AddValidatorsFromAssemblyContaining<GetReport>();
}

static void ConfigureApplication(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthorization();
    app.MapControllers();
}
