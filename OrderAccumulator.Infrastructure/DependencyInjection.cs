using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Infrastructure.Data;
using OrderAccumulator.Infrastructure.Policies;
using OrderAccumulator.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Orderdb")));
            services.AddScoped<IExposureRepository, ExposureRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddHostedService<Fix.FixServer>();
            services.AddSingleton<Fix.FixApplication>();
            services.AddSingleton(RetryPolicy.Create());

            return services;
        }
    }
}
