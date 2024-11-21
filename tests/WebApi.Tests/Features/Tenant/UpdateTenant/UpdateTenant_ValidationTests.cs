namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.UpdateTenant;

[Collection("Using test API")]
public class UpdateTenant_ValidationTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;

    [Fact(DisplayName = "PATCH tenants/{id} when tenant doesn't exist should return 404")]
    public async Task Patch_WhenTenantDoesntExist_ShouldReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new { };

        // Act
        var response = await Fixture.Api.PatchAsJsonAsync($"tenants/{tenantId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "PATCH tenants/{id} when name is empty string should return 400")]
    public async Task Patch_WhenNameIsEmptyString_ShouldReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.CreateVersion7();
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Id = tenantId, Name = "Test" });
        await context.SaveChangesAsync(CancellationToken.None);

        var request = new { Name = "" };

        // Act
        var response = await Fixture.Api.PatchAsJsonAsync($"tenants/{tenantId}", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
