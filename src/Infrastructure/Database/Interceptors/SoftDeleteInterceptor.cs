using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.Infrastructure.Database.Interceptors;
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly ITime _time;

    public SoftDeleteInterceptor(ITime time)
    {
        _time = time;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
    {
        var changeTrackerEntries = eventData.Context?.ChangeTracker.Entries() ?? [];

        foreach (var entry in changeTrackerEntries)
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete entity }) continue;
            entry.State = EntityState.Modified;
            entity.IsDeleted = true;
            entity.DeletedAt = _time.Now;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
