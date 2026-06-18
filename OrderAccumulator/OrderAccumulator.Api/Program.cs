using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using OrderAccumulator.Api.Middleware;
using OrderAccumulator.Application;
using OrderAccumulator.Infrastructure;
using OrderAccumulator.Infrastructure.Data;
using OrderAccumulator.Infrastructure.Telemetry;
using QuickFix.Fields;
using Serilog;

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<OrderDbContext>();

        context.Database.Migrate();
        Console.WriteLine("--> Migrations aplicadas com sucesso no PostgreSQL!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Erro ao rodar migrations: {ex.Message}");
    }
}

app.Run();

public partial class Program
{
}