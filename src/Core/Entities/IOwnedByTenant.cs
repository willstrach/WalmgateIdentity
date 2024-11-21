namespace WalmgateIdentity.Core.Entities;

public interface IOwnedByTenant
{
    Guid TenantId { get; set; }
}
