using Microsoft.AspNetCore.Mvc;
using WalmgateIdentity.WebApi.Models;

namespace WalmgateIdentity.WebApi.Helpers;

public static class ResponseMappings
{
    public async static Task<IResult> ToApiResult<TData>(this Task<Response<TData>> responseTask)
    {
        var response = await responseTask;

        if (response.Status == ResponseStatus.BadRequest) return response.ToValidationProblemDetails();
        if (!response.IsSuccessful) return response.ToProblemDetails();

        return response.Status switch
        {
            ResponseStatus.OK => Results.Ok(response.Data),
            ResponseStatus.NoContent => Results.NoContent(),
            _ => throw new NotImplementedException($"Response status {response.Status} is not supported.")
        };
    }

    private static IResult ToValidationProblemDetails<TData>(this Response<TData> response)
    {
        var validationProblemDetails = new ValidationProblemDetails(response.Errors)
        {
            Status = (int)response.Status,
            Title = "Bad Request",
            Detail = "One or more validation errors occurred.",
            Type = $"https://httpstatuses.com/{(int)response.Status}"
        };

        return Results.ValidationProblem(response.Errors, title: "Bad Request", statusCode: (int)response.Status,
            type: $"https://httpstatuses.com/{(int)response.Status}", detail: "One or more validation errors occurred.");
    }

    private static IResult ToProblemDetails<TData>(this Response<TData> response)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Status = (int)response.Status;

        problemDetails.Title = response.Status switch
        {
            ResponseStatus.Unauthorized => "Unauthorized",
            ResponseStatus.Forbidden => "Forbidden",
            ResponseStatus.NotFound => "Not Found",
            ResponseStatus.Conflict => "Conflict",
            ResponseStatus.InternalServerError => "Internal Server Error",
            ResponseStatus.NotImplemented => "Not Implemented",
            _ => throw new NotImplementedException($"Response status {response.Status} is not supported.")
        };

        problemDetails.Detail = response.Errors.Values.FirstOrDefault()?.FirstOrDefault();
        problemDetails.Type = $"https://httpstatuses.com/{(int)response.Status}";

        return Results.Problem(problemDetails);
    }
}
