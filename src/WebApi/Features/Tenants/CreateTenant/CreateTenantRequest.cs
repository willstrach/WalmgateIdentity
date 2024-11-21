using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalmgateIdentity.WebApi.Models;

namespace WalmgateIdentity.WebApi.Features.Tenants.CreateTenant;

public record CreateTenantRequest : IRequest<Response<TenantVm>>
{
    public string Name { get; set; } = string.Empty;
}

public class CreateTenantEndpoint : IEndpoint
{
    public void Map(WebApplication app) => app.MapPost("tenants", CreateTenant)
        .WithName("CreateTenant")
        .WithDisplayName("Create Tenant")
        .WithDescription("Creates a new tenant.")
        .WithTags("Tenants")
        .Accepts<CreateTenantRequest>("application/json")
        .Produces<TenantVm>(200);

    private async Task<IResult> CreateTenant([FromServices] ISender sender, [FromBody] CreateTenantRequest request)
        => await sender.Send(request).ToApiResult();
}
