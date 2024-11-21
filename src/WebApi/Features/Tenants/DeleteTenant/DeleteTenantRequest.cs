
using Microsoft.AspNetCore.Mvc;

namespace WalmgateIdentity.WebApi.Features.Tenants.DeleteTenant;

public record DeleteTenantRequest : IRequest<Response<Empty>>
{
    public Guid TenantId { get; set; }
}

public class DeleteTenantEndpoint : IEndpoint
{
    public void Map(WebApplication app) => app.MapDelete("tenants/{tenantId:guid}", DeleteTenant)
        .Produces(StatusCodes.Status204NoContent)
        .WithName("DeleteTenant")
        .WithDisplayName("Delete Tenant")
        .WithDescription("Deletes a tenant.")
        .WithTags("Tenants");

    private async Task<IResult> DeleteTenant([FromServices] ISender sender, [FromRoute] Guid tenantId, CancellationToken cancellationToken)
        => await sender.Send(new DeleteTenantRequest { TenantId = tenantId }, cancellationToken).ToApiResult();
}
