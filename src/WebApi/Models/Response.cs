
namespace WalmgateIdentity.WebApi.Models;

public record Response<TData> : IResponse
{
    public bool IsSuccessful { get; set; }
    public ResponseStatus Status { get; set; }
    public TData? Data { get; set; } = default;
    public Dictionary<string, string[]> Errors { get; set; } = [];

    public static implicit operator Response<TData>(Response<Empty> emptyResponse) => new()
    {
        IsSuccessful = emptyResponse.IsSuccessful,
        Status = emptyResponse.Status,
        Errors = emptyResponse.Errors,
    };
}

public record Empty();
