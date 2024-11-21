using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Entities;

namespace WalmgateIdentity.Core.Interfaces;

public interface IDatabaseContext
{
    DbSet<Tenant> Tenants { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
