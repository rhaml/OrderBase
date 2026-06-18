using OpenTelemetry.Trace;
using Serilog;

using OrderAccumulator.Api.Middleware;
using OrderAccumulator.Application;
using OrderAccumulator.Infrastructure;
using OrderAccumulator.Infrastructure.Telemetry;

var builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (ctx, cfg) =>
    {
        cfg
            .ReadFrom.Configuration(
                ctx.Configuration)
            .WriteTo.Console();
    });

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer();

builder.Services
    .AddSwaggerGen();

builder.Services
    .AddApplication();

builder.Services
    .AddInfrastructure(
        builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration
            .GetConnectionString(
                "Orderdb")!);

builder.Services
    .AddOpenTelemetry()
    .WithTracing(trace =>
    {
        trace
            .AddAspNetCoreInstrumentation()
            .AddSource("OrderAccumulator")
            .AddConsoleExporter();
    });

var app = builder.Build();

app.UseMiddleware<
    ExceptionMiddleware>();

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();

app.MapHealthChecks(
    "/health");

app.Run();

public partial class Program
{
}