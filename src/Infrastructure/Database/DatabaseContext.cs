using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.Infrastructure.Database.Configurations;
using WalmgateIdentity.Infrastructure.Database.Interceptors;

namespace WalmgateIdentity.Infrastructure.Database;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<Tenant> Tenants { get; set; } = null!;

    private readonly ICurrentUser _currentUser;
    private readonly ITime _time;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, ICurrentUser currentUser, ITime time) : base(options)
    {
        _currentUser = currentUser;
        _time = time;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

        modelBuilder.HasGlobalQueryFilter<ISoftDelete>(entity => !entity.IsDeleted);
        modelBuilder.HasGlobalQueryFilter<IOwnedByTenant>(entity => entity.TenantId == _currentUser.TenantId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor(_time));
    }
}
