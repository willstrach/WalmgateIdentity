using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WalmgateIdentity.WebApi.Helpers;

namespace WalmgateIdentity.WebApi.Tests.Helpers;

public class ResponseMappingsTests
{
    [Fact]
    public async Task ToApiResult_WithOkResponse_ShouldReturnOkResult()
    {
        // Given
        var data = new TestData(1, "Test");
        var response = Response.OK(data);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<Ok<TestData>>();
        result.As<Ok<TestData>>().Value.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task ToApiResult_WithNoContentResponse_ShouldReturnNoContentResult()
    {
        // Given
        var response = Response.NoContent();

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task ToApiResult_WithBadRequestResponse_ShouldReturnValidationProblemDetails()
    {
        // Given
        var errors = new Dictionary<string, string[]>
        {
            { "Name", [ "Name is required" ] }
        };
        var response = Response.BadRequest(errors);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<HttpValidationProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<HttpValidationProblemDetails>();
        problemDetails.Errors.Keys.Should().HaveCount(1);
        problemDetails.Type.Should().Be($"https://httpstatuses.com/400");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be("Bad Request");
        problemDetails.Detail.Should().Be("One or more validation errors occurred.");
    }

    [Fact]
    public async Task ToApiResult_WithUnauthorizedResponse_ShouldReturnUnauthorizedProblemDetails()
    {
        // Given
        var errorMessage = "You are not authorized to access this resource.";
        var response = Response.Unauthorized(errorMessage);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/401");
        problemDetails.Status.Should().Be(401);
        problemDetails.Title.Should().Be("Unauthorized");
        problemDetails.Detail.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ToApiResult_WithForbiddenResponse_ShouldReturnForbiddenProblemDetails()
    {
        // Given
        var errorMessage = "You are forbidden from accessing this resource.";
        var response = Response.Forbidden(errorMessage);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/403");
        problemDetails.Status.Should().Be(403);
        problemDetails.Title.Should().Be("Forbidden");
        problemDetails.Detail.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ToApiResult_WithNotFoundResponse_ShouldReturnNotFoundProblemDetails()
    {
        // Given
        var errorMessage = "Resource not found.";
        var response = Response.NotFound(errorMessage);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/404");
        problemDetails.Status.Should().Be(404);
        problemDetails.Title.Should().Be("Not Found");
        problemDetails.Detail.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ToApiResult_WithConflictResponse_ShouldReturnConflictProblemDetails()
    {
        // Given
        var errorMessage = "Row version mismatch.";
        var response = Response.Conflict(errorMessage);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/409");
        problemDetails.Status.Should().Be(409);
        problemDetails.Title.Should().Be("Conflict");
        problemDetails.Detail.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ToApiResult_WithInternalServerErrorResponse_ShouldReturnInternalServerErrorProblemDetails()
    {
        // Given
        var errorMessage = "An unexpected error occurred.";
        var response = Response.InternalServerError(errorMessage);

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/500");
        problemDetails.Status.Should().Be(500);
        problemDetails.Title.Should().Be("Internal Server Error");
        problemDetails.Detail.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ToApiResult_WithNotImplementedResponse_ShouldReturnNotImplementedProblemDetails()
    {
        // Given
        var response = Response.NotImplemented();

        // When
        var result = await Task.FromResult(response).ToApiResult();

        // Then
        result.Should().BeOfType<ProblemHttpResult>();
        result.As<ProblemHttpResult>().ProblemDetails.Should().BeOfType<ProblemDetails>();

        var problemDetails = result.As<ProblemHttpResult>().ProblemDetails.As<ProblemDetails>();
        problemDetails.Type.Should().Be($"https://httpstatuses.com/501");
        problemDetails.Status.Should().Be(501);
        problemDetails.Title.Should().Be("Not Implemented");
    }

    record TestData(int Id, string Name);
}
