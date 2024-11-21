using WalmgateIdentity.WebApi.Models;

namespace WalmgateIdentity.WebApi.Helpers;

public static class Response
{
    public static Response<TData> OK<TData>(TData data) => Success(data) with { Status = ResponseStatus.OK };
    public static Response<Empty> NoContent() => Success<Empty>(null) with { Status = ResponseStatus.NoContent };

    public static Response<Empty> BadRequest(string location, string message) => Failure(location, message) with { Status = ResponseStatus.BadRequest };
    public static Response<Empty> BadRequest(Dictionary<string, string[]> errors) => Failure(errors) with { Status = ResponseStatus.BadRequest };

    public static Response<Empty> Unauthorized(string message) => Failure(string.Empty, message) with { Status = ResponseStatus.Unauthorized };
    public static Response<Empty> Forbidden(string message) => Failure(string.Empty, message) with { Status = ResponseStatus.Forbidden };
    public static Response<Empty> NotFound(string message) => Failure(string.Empty, message) with { Status = ResponseStatus.NotFound };
    public static Response<Empty> Conflict(string message) => Failure(string.Empty, message) with { Status = ResponseStatus.Conflict };

    public static Response<Empty> InternalServerError(string message) => Failure(string.Empty, message) with { Status = ResponseStatus.InternalServerError };
    public static Response<Empty> NotImplemented() => Failure([]) with { Status = ResponseStatus.NotImplemented };

    private static Response<TData> Success<TData>(TData? data) => new()
    {
        IsSuccessful = true,
        Status = ResponseStatus.NotImplemented,
        Data = data
    };

    private static Response<Empty> Failure(Dictionary<string, string[]> errors) => new()
    {
        IsSuccessful = false,
        Status = ResponseStatus.NotImplemented,
        Errors = errors,
    };

    private static Response<Empty> Failure(string location, string message)
        => Failure(new() { { location, [ message ] } });
}

public static class GenericResponse<TResponse> where TResponse : IResponse, new()
{
    public static TResponse BadRequest(string location, string message) => Failure(ResponseStatus.BadRequest, location, message);
    public static TResponse BadRequest(Dictionary<string, string[]> errors) => Failure(ResponseStatus.BadRequest, errors);

    public static TResponse Unauthorized(string message) => Failure(ResponseStatus.Unauthorized, string.Empty, message);
    public static TResponse Forbidden(string message) => Failure(ResponseStatus.Forbidden, string.Empty, message);
    public static TResponse NotFound(string message) => Failure(ResponseStatus.NotFound, string.Empty, message);
    public static TResponse Conflict(string message) => Failure(ResponseStatus.Conflict, string.Empty, message);

    public static TResponse InternalServerError(string message) => Failure(ResponseStatus.InternalServerError, string.Empty, message);
    public static TResponse NotImplemented(string message) => Failure(ResponseStatus.NotImplemented, string.Empty, message);

    private static TResponse Failure(ResponseStatus status, Dictionary<string, string[]> errors) => new()
    {
        IsSuccessful = false,
        Status = status,
        Errors = errors,
    };

    private static TResponse Failure(ResponseStatus status, string location, string message)
        => Failure(status, new() { { location, [ message ] } });
}
