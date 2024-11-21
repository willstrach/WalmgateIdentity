using Microsoft.EntityFrameworkCore;

namespace WalmgateIdentity.WebApi.Helpers;

public static class PagedListExtensions
{
    public static async Task<PagedList<TItem>> ToPagedListAsync<TItem>(this IQueryable<TItem> query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<TItem>()
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = items
        };
    }
}
