using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.CommonModule.Data;
using VerticalSliceExample.CommonModule.PipelineBehaviors;
using VerticalSliceExample.CommonModule.Repository;
using VerticalSliceExample.CommonModule.Repository.Interface;
using VerticalSliceExample.ReportModule.Features;
using VerticalSliceExample.ReportModule.Repositories;
using VerticalSliceExample.ReportModule.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/CommonModule/Pages/{2}/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/CommonModule/Pages/Shared/{0}.cshtml");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MediatRTestConnectionString")));
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblyContaining<Program>()
        .AddOpenBehavior(typeof(LoggingBehavior<,>))
        .AddBehavior<IPipelineBehavior<GetReport, IResponse>, ValidationBehavior<GetReport, IResponse>>()
        .AddBehavior<IPipelineBehavior<CreateReport, IResponse>, ValidationBehavior<CreateReport, IResponse>>());
builder.Services.AddValidatorsFromAssemblyContaining<GetReport>();

var app = builder.Build();

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

app.Run();
