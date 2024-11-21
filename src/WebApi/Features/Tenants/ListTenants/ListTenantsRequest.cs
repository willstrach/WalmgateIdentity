
using Microsoft.AspNetCore.Mvc;

namespace WalmgateIdentity.WebApi.Features.Tenants.ListTenants;

public record ListTenantsRequest : IPagedRequest, IRequest<Response<PagedList<TenantVm>>>
{
    public string? Search { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}

public class ListTenantsEndpoint : IEndpoint
{
    public void Map(WebApplication app) => app.MapGet("/tenants", ListTenants);

    private static async Task<IResult> ListTenants([FromServices] ISender sender, [AsParameters] ListTenantsRequest request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken).ToApiResult();
}
