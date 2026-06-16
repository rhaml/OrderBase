using OrderAccumulator.Fix;
using OrderAccumulator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ExposureService>();
builder.Services.AddSingleton<FixApplication>();
builder.Services.AddHostedService<FixServer>();

var app = builder.Build();

app.MapGet("/health", () =>
        {Results.Ok("UP");
})
.WithName("GetWeatherForecast");

app.Run();
