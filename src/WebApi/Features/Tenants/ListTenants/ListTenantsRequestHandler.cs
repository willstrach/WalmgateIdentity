using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Features.Tenants.ListTenants;

public class ListTenantsRequestHandler : IRequestHandler<ListTenantsRequest, Response<PagedList<TenantVm>>>
{
    private readonly IDatabaseContext _context;

    public ListTenantsRequestHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<PagedList<TenantVm>>> Handle(ListTenantsRequest request, CancellationToken cancellationToken)
    {
        var pagedTenants = await _context.Tenants
            .Where(tenant => String.IsNullOrEmpty(request.Search) || tenant.Name.Contains(request.Search))
            .Select(tenant => tenant.ToVm())
            .ToPagedListAsync(request.Page ?? 1, request.PageSize ?? 20, cancellationToken);

        return Response.OK(pagedTenants);
    }
}
