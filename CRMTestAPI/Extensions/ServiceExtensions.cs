using System.Text;
using Entities;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using ActionFilters;
using LoggerService.Contracts;
using Repositories.Contracts;

namespace CRMTestAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void ConfigureConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppConfig>(config.GetSection("authentication"));
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(o => o.UseMySql(config["MysqlConnection:ConnectionString"]));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:JwtSecretKey"]))
                };
            });
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<EntityIsValidActionFilter>();
            services.AddScoped<EntityExistsActionFilter<User>>();
            services.AddScoped<EntityExistsActionFilter<Role>>();
            services.AddScoped<EntityExistsActionFilter<Customer>>();
        }
    }
}