using System.Net;
using System.Net.Http.Json;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.CreateTenant;

[Collection("Using test API")]
public class CreateTenant_ValidationTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;


    [Fact( DisplayName = "POST /tenants with empty string name should return 400" )]
    public async Task Post_WithEmptyStringName_ShouldReturn400()
    {
        // Given
        var request = new { Name = string.Empty };

        // When
        var response = await Fixture.Api.PostAsJsonAsync("/tenants", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact( DisplayName = "POST /tenants with long name should return 400" )]
    public async Task Post_WithLongName_ShouldReturn400()
    {
        // Given
        var request = new { Name = "This name is longer than 50 characters so should return a 400" };

        // When
        var response = await Fixture.Api.PostAsJsonAsync("/tenants", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact( DisplayName = "POST /tenants with name that already exists should return 400" )]
    public async Task Post_WithExistingName_ShouldReturn400()
    {
        // Given
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Name = "Existing Tenant" });
        await context.SaveChangesAsync(CancellationToken.None);
        
        var request = new { Name = "Existing Tenant" };

        // When
        var response = await Fixture.Api.PostAsJsonAsync("/tenants", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
