using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Middleware.Auth;

public class CurrentUserService : ICurrentUser
{
    public Guid Id => throw new NotImplementedException();

    public string Email => throw new NotImplementedException();

    public bool IsAuthenticated => throw new NotImplementedException();

    public Guid TenantId => throw new NotImplementedException();
}
