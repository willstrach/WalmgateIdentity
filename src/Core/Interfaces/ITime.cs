namespace WalmgateIdentity.Core.Interfaces;

public interface ITime
{
    DateTimeOffset Now { get; }
}
