namespace WalmgateIdentity.Core.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
    Guid TenantId { get; }
}