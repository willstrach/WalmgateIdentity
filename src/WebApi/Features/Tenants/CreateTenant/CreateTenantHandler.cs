namespace WalmgateIdentity.WebApi.Features.Tenants.CreateTenant;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.Core.Entities;
using WalmgateIdentity.Core.Interfaces;

public class CreateTenantHandler : IRequestHandler<CreateTenantRequest, Response<TenantVm>>
{
    private readonly IDatabaseContext _context;

    public CreateTenantHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response<TenantVm>> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenantExists = await _context.Tenants.AnyAsync(x => x.Name == request.Name, cancellationToken);
        if (tenantExists) return Response.BadRequest(nameof(request.Name), $"Tenant already exists with name {request.Name}");

        var tenant = new Tenant()
        {
            Name = request.Name
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return Response.OK(tenant.ToVm());
    }
}
