
using Microsoft.AspNetCore.Mvc;

namespace WalmgateIdentity.WebApi.Features.Tenants.GetTenant;

public record GetTenantRequest(Guid TenantId) : IRequest<Response<TenantVm>>;

public class GetTenantEndpoint : IEndpoint
{
    public void Map(WebApplication app) => app.MapGet("/tenants/{tenantId:guid}", GetTenant)
        .Produces<TenantVm>(StatusCodes.Status200OK)
        .WithName("GetTenant")
        .WithDisplayName("Get Tenant")
        .WithDescription("Gets a tenant.")
        .WithTags("Tenants");

    private static async Task<IResult> GetTenant([FromServices] ISender sender, [FromRoute] Guid tenantId, CancellationToken cancellationToken)
        => await sender.Send(new GetTenantRequest(tenantId), cancellationToken).ToApiResult();
}

