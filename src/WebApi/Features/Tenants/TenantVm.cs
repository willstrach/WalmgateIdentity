using WalmgateIdentity.Core.Entities;

namespace WalmgateIdentity.WebApi.Features.Tenants;

public record TenantVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static class TenantVmMappings
{
    public static TenantVm ToVm(this Tenant tenant)
    {
        return new TenantVm
        {
            Id = tenant.Id,
            Name = tenant.Name
        };
    }
}
