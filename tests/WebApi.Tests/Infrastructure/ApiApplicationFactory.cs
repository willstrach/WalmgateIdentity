using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.Infrastructure.Database;

namespace WalmgateIdentity.WebApi.Tests.Infrastructure;

public class ApiApplicationFactory(string databaseConnectionString) : WebApplicationFactory<Program>
{
    public readonly TestTime Time = new();
    public readonly TestCurrentUser CurrentUser = new();
    public IServiceScope CreateScope() => Services.CreateScope();
    public IDatabaseContext CreateDatabaseContext() => CreateScope().ServiceProvider.GetRequiredService<IDatabaseContext>();

    private readonly string _databaseConnectionString = databaseConnectionString;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => {
            services.TryRemoveService<ITime>();
            services.AddSingleton<ITime, TestTime>(serviceProvider => Time);

            services.TryRemoveService<ICurrentUser>();
            services.AddSingleton<ICurrentUser, TestCurrentUser>(serviceProvider => CurrentUser);

            services.TryRemoveService<DbContextOptions<DatabaseContext>>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(_databaseConnectionString);
            });
        });
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryRemoveService<TExisitingService>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(TExisitingService));
        if (descriptor is null) return services;

        services.Remove(descriptor);
        return services;
    }
}
