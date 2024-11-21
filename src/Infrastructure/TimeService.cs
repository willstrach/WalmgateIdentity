using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.Infrastructure;

public class TimeService : ITime
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
