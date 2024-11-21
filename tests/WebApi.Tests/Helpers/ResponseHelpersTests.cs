using WalmgateIdentity.WebApi.Helpers;
using WalmgateIdentity.WebApi.Models;

namespace WalmgateIdentity.WebApi.Tests.Helpers;

public class ResponseHelpersTests
{
    [Fact]
    public void OK_WithData_ShouldReturnSuccessfulResultWithOkStatus()
    {
        // Given
        var data = new TestData(1, "Test");

        // When
        var response = Response.OK(data);

        // Then
        response.IsSuccessful.Should().BeTrue();
        response.Status.Should().Be(ResponseStatus.OK);
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void NoContent_WithNoData_ShouldReturnSuccessfulResultWithNoContentStatus()
    {
        // When
        var response = Response.NoContent();

        // Then
        response.IsSuccessful.Should().BeTrue();
        response.Status.Should().Be(ResponseStatus.NoContent);
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void BadRequest_WithErrors_ShouldReturnFailedResultWithBadRequestStatus()
    {
        // Given
        var errors = new Dictionary<string, string[]>
        {
            { "Name", [ "Name is required" ] }
        };

        // When
        Response<TestData> response = Response.BadRequest(errors);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.BadRequest);
        response.Errors.Keys.Should().HaveCount(1);

        var nameErrors = response.Errors["Name"];
        nameErrors.Should().HaveCount(1);
        nameErrors.Should().Contain("Name is required");
    }

    [Fact]
    public void BadRequest_WithIndividualError_ShouldReturnFailedResultWithBadRequestStatus()
    {
        // When
        Response<TestData> response = Response.BadRequest("Name", "Name is required");

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.BadRequest);
        response.Errors.Keys.Should().HaveCount(1);

        var nameErrors = response.Errors["Name"];
        nameErrors.Should().HaveCount(1);
        nameErrors.Should().Contain("Name is required");
    }

    [Fact]
    public void Unauthorized_WithMessage_ShouldReturnFailedResultWithUnauthorizedStatus()
    {
        // Given
        var message = "You are not authorized to access this resource";

        // When
        var response = Response.Unauthorized(message);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.Unauthorized);
        response.Errors.Keys.Should().HaveCount(1);
        response.Errors.Keys.Should().Contain(string.Empty);

        var errors = response.Errors[string.Empty];
        errors.Should().HaveCount(1);
        errors.Should().Contain(message);
    }

    [Fact]
    public void Forbidden_WithMessage_ShouldReturnFailedResultWithForbiddenStatus()
    {
        // Given
        var message = "You are forbidden to access this resource";

        // When
        var response = Response.Forbidden(message);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.Forbidden);
        response.Errors.Keys.Should().HaveCount(1);
        response.Errors.Keys.Should().Contain(string.Empty);

        var errors = response.Errors[string.Empty];
        errors.Should().HaveCount(1);
        errors.Should().Contain(message);
    }

    [Fact]
    public void NotFound_WithMessage_ShouldReturnFailedResultWithNotFoundStatus()
    {
        // Given
        var message = "Resource not found";

        // When
        var response = Response.NotFound(message);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.NotFound);
        response.Errors.Keys.Should().HaveCount(1);
        response.Errors.Keys.Should().Contain(string.Empty);

        var errors = response.Errors[string.Empty];
        errors.Should().HaveCount(1);
        errors.Should().Contain(message);
    }

    [Fact]
    public void Conflict_WithMessage_ShouldReturnFailedResultWithNotFoundStatus()
    {
        // Given
        var message = "Row version mismatch";

        // When
        var response = Response.Conflict(message);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.Conflict);
        response.Errors.Keys.Should().HaveCount(1);
        response.Errors.Keys.Should().Contain(string.Empty);

        var errors = response.Errors[string.Empty];
        errors.Should().HaveCount(1);
        errors.Should().Contain(message);
    }

    [Fact]
    public void InternalServerError_WithMessage_ShouldReturnFailedResultWithInternalServerErrorStatus()
    {
        // Given
        var message = "An error occurred while processing your request";

        // When
        var response = Response.InternalServerError(message);

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.InternalServerError);
        response.Errors.Keys.Should().HaveCount(1);
        response.Errors.Keys.Should().Contain(string.Empty);

        var errors = response.Errors[string.Empty];
        errors.Should().HaveCount(1);
        errors.Should().Contain(message);
    }

    [Fact]
    public void NotImplemented_WithMessage_ShouldReturnFailedResultWithInternalServerErrorStatus()
    {
        // When
        var response = Response.NotImplemented();

        // Then
        response.IsSuccessful.Should().BeFalse();
        response.Status.Should().Be(ResponseStatus.NotImplemented);
    }

    record TestData(int Id, string Name);
}
