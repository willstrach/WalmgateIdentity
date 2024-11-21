
using Microsoft.AspNetCore.Mvc;

namespace WalmgateIdentity.WebApi.Features.Tenants.UpdateTenant;

public record UpdateTenantRequest : IRequest<Response<TenantVm>>
{
    internal Guid Id { get; set; }
    public string? Name { get; set; }
}

public class UpdateTenantEndpoint : IEndpoint
{
    public void Map(WebApplication app) => app.MapPatch("tenants/{tenantId:guid}", UpdateTenant)
        .WithName("UpdateTenant")
        .WithDisplayName("Update Tenant")
        .WithDescription("Updates a tenant.")
        .WithTags("Tenants")
        .Accepts<UpdateTenantRequest>("application/json")
        .Produces<TenantVm>(200);

    private async Task<IResult> UpdateTenant([FromServices] ISender sender, [FromRoute] Guid tenantId, [FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
        => await sender.Send(request with { Id = tenantId }, cancellationToken).ToApiResult();
}
