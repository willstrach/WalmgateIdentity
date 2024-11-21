using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.ListTenants;
public class ListTenants_ValidationTests(ApiApplicationFixture fixture) : UsingApi(fixture)
{
    [Fact(DisplayName = "GET /tenants when page is less than 1 should return 400")]
    public async Task Get_WhenPageIsLessThan1_ShouldReturn400()
    {
        // Given
        var request = new Dictionary<string, string?>
        {
            { "page", "0" },
            { "pageSize", "10" }
        };

        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody?.Errors.Should().ContainKey("Page");
    }

    [Fact(DisplayName = "GET /tenants when page size is less than 1 should return 400")]
    public async Task Get_WhenPageSizeIsLessThan1_ShouldReturn400()
    {
        // Given
        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "0" }
        };
        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody?.Errors.Should().ContainKey("PageSize");
    }

    [Fact(DisplayName = "GET /tenants when page size is greater than 100 should return 400")]
    public async Task Get_WhenPageSizeIsGreaterThan100_ShouldReturn400()
    {
        // Given
        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "101" }
        };
        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody?.Errors.Should().ContainKey("PageSize");
    }

    [Fact(DisplayName = "GET /tenants when search is longer than 50 characters should return 400")]
    public async Task Get_WhenSearchIsLongerThan50Characters_ShouldReturn400()
    {
        // Given
        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "10" },
            { "search", new string('a', 51) }
        };
        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody?.Errors.Should().ContainKey("Search");
    }
}
