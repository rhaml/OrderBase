using OpenTelemetry.Trace;
using OrderGenerator.Api.Middleware;
using OrderGenerator.Application;
using OrderGenerator.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, cfg) => 
    {
        cfg.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console();
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddHealthChecks();
builder.Services.AddOpenTelemetry().WithTracing(trace =>
    {
        trace.AddAspNetCoreInstrumentation()
             .AddSource("OrderGenerator")
             .AddConsoleExporter();
    });

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}
