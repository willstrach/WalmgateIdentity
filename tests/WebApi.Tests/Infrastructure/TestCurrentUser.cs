using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Tests.Infrastructure;

public class TestCurrentUser : ICurrentUser
{
    public Guid Id => Guid.NewGuid();
    public string Email => string.Empty;
    public bool IsAuthenticated => true;
    public Guid TenantId => Guid.NewGuid();
}
