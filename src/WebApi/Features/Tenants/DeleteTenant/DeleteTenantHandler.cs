
using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Features.Tenants.DeleteTenant;

public class DeleteTenantHandler : IRequestHandler<DeleteTenantRequest, Response<Empty>>
{
    private readonly IDatabaseContext _context;

    public DeleteTenantHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<Empty>> Handle(DeleteTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants.FindAsync(request.TenantId);
        
        if (tenant is null) return Response.NotFound($"Tenant not found with ID {request.TenantId}");

        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return Response.NoContent();
    }
}
