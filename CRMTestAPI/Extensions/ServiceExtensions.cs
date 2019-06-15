using Contracts;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories;

namespace CRMTestAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(config["mysqlconnection:connectionString"]));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
