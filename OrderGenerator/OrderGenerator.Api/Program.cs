using OpenTelemetry.Trace;
using OrderGenerator.Api.Middleware;
using OrderGenerator.Application;
using OrderGenerator.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
                      });
});

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

app.UseRouting();
app.UseCors(allowSpecificOrigins);

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}
