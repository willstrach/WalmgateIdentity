namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.GetTenant;

[Collection("Using test API")]
public class GetTenant_ValidationTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;

    [Fact( DisplayName = "GET /tenants/{tenantId} with empty tenant should return 404")]
    public async Task Get_WhenTenantIdIsEmptyGuid_ShouldReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.Empty;

        // Act
        var response = await Fixture.Api.GetAsync($"tenants/{tenantId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "GET /tenants/{tenantId} when tenant does not exist should return 404")]
    public async Task Get_WhenTenantDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Name = "Tenant 1" });
        await context.SaveChangesAsync(CancellationToken.None);

        var tenantId = Guid.NewGuid();

        // Act
        var response = await Fixture.Api.GetAsync($"tenants/{tenantId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "GET /tenants/{tenantId} when tenant is deleted should return 404")]
    public async Task Get_WhenTenantIsDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.CreateVersion7();
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Id = tenantId, Name = "Tenant 1", IsDeleted = true });
        await context.SaveChangesAsync(CancellationToken.None);

        // Act
        var response = await Fixture.Api.GetAsync($"tenants/{tenantId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
