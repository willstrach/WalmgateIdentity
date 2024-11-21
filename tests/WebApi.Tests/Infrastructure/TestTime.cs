using WalmgateIdentity.Core.Interfaces;

namespace WalmgateIdentity.WebApi.Tests.Infrastructure;

public class TestTime : ITime
{
    private DateTimeOffset _now = new DateTime(200, 1, 1);

    public DateTimeOffset Now => _now;

    public void SetTime(DateTime time)
    {
        _now = time;
    }
}
