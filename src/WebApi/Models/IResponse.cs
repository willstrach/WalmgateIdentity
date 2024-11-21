namespace WalmgateIdentity.WebApi.Models;

public interface IResponse
{
    bool IsSuccessful { get; set; }
    ResponseStatus Status { get; set; }
    Dictionary<string, string[]> Errors { get; set; }
}
