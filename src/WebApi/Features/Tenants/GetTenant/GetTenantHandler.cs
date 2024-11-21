using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Features.Tenants.GetTenant;

public class GetTenantHandler : IRequestHandler<GetTenantRequest, Response<TenantVm>>
{
    private readonly IDatabaseContext _databaseContext;

    public GetTenantHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Response<TenantVm>> Handle(GetTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _databaseContext.Tenants
            .AsNoTracking()
            .Where(tenant => tenant.Id == request.TenantId)
            .Select(tenant => tenant.ToVm())
            .FirstOrDefaultAsync(cancellationToken);
        
        return tenant is not null
            ? Response.OK(tenant)
            : Response.NotFound($"Tenant not found with ID {request.TenantId}");
    }
}
