using WalmgateIdentity.WebApi.Features.Tenants;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.GetTenant;

[Collection("Using test API")]
public class GetTenant_HappyPathTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;

    [Fact(DisplayName = "GET /tenants/{tenantId} when tenant exists should return tenant")]
    public async Task Get_WhenTenantExists_ShouldReturnTenant()
    {
        // Arrange
        var tenantId = Guid.CreateVersion7();
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Id = tenantId, Name = "Tenant 1" });
        await context.SaveChangesAsync(CancellationToken.None);

        // Act
        var response = await Fixture.Api.GetAsync($"tenants/{tenantId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tenant = await response.Content.ReadFromJsonAsync<TenantVm>();
        tenant.Should().NotBeNull();
        tenant!.Id.Should().Be(tenantId);
        tenant.Name.Should().Be("Tenant 1");
    }
}
