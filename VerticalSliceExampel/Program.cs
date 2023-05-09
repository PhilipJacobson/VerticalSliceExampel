using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceExampel.CommonModule;
using VerticalSliceExampel.CommonModule.Data;
using VerticalSliceExampel.CommonModule.PipelineBehaviors;
using VerticalSliceExampel.CommonModule.Repository;
using VerticalSliceExampel.CommonModule.Repository.Interface;
using VerticalSliceExampel.ReportModule.Features;
using VerticalSliceExampel.ReportModule.Repositories;
using VerticalSliceExampel.ReportModule.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

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
