using Microsoft.Extensions.DependencyInjection;
using OrderGenerator.Application.Interfaces;
using OrderGenerator.Infrastructure.Fix;

namespace OrderGenerator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ExecutionReportStore>();
            services.AddSingleton<FixApplication>();
            services.AddSingleton<IFixGateway, FixGateway>();
            services.AddHostedService<FixClient>();
            return services;
        }

    }
}
