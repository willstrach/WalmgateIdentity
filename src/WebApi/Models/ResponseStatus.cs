namespace WalmgateIdentity.WebApi.Models;

public enum ResponseStatus
{
    // Successful
    OK = 200,
    NoContent = 204,

    // Client errors
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,


    // Server errors
    InternalServerError = 500,
    NotImplemented = 501,
}
