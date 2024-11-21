using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Features.Tenants.UpdateTenant;

public class UpdateTenantHandler : IRequestHandler<UpdateTenantRequest, Response<TenantVm>>
{
    private readonly IDatabaseContext _context;

    public UpdateTenantHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<TenantVm>> Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenants = await _context.Tenants
            .Where(tenant => tenant.Id == request.Id || tenant.Name == request.Name)
            .ToListAsync(cancellationToken);

        var tenant = tenants.FirstOrDefault(tenant => tenant.Id == request.Id);

        if (tenant is null) return Response.NotFound($"Tenant not found with id {request.Id}");
        if (tenants.Count > 1) return Response.BadRequest(nameof(request.Name), $"Tenant already exists with name {request.Name}");

        tenant.Name = request.Name ?? tenant.Name;
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return Response.OK(tenant.ToVm());
    }
}
