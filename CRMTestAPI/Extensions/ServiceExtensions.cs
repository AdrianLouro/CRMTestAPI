using Contracts;
using Microsoft.Extensions.DependencyInjection;
using LoggerService;

namespace CRMTestAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
