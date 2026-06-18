
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Application.Behaviors;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Application.Processors;
using System.Reflection;

namespace OrderAccumulator.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IOrderProcessor, OrderProcessor>();
            services.AddScoped<Interfaces.IExposureService, Services.ExposureService>();
            return services;
        }
    }
}
