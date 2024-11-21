using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.Infrastructure.Database;

namespace WalmgateIdentity.Infrastructure;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITime, TimeService>();
        services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
