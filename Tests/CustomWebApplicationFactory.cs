using CRMTestAPI;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabase");
                    options.UseInternalServiceProvider(
                        new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider());
                });

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.EnsureCreated();
                    Seed(dbContext);
                }
            });
        }

        private void Seed(AppDbContext dbContext)
        {
            new UserSeeder(dbContext).Seed();
            new RoleSeeder(dbContext).Seed();
        }
    }
}