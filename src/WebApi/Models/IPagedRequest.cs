namespace WalmgateIdentity.WebApi.Models;

public interface IPagedRequest
{
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}
