using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.WebApi.Features.Tenants;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.UpdateTenant;

[Collection("Using test API")]
public class UpdateTenant_HappyPathTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;

    [Fact(DisplayName = "PATCH tenants/{id} when name is provided should return 200")]
    public async Task Patch_WhenNameIsProvided_ShouldReturnNoContent()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Id = tenantId, Name = "Test" });
        await context.SaveChangesAsync(CancellationToken.None);
        var request = new { Name = "Test 2" };

        // Act
        var response = await Fixture.Api.PatchAsJsonAsync($"tenants/{tenantId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadFromJsonAsync<TenantVm>();
        responseContent!.Name.Should().Be("Test 2");

        context = Fixture.CreateDatabaseContext();
        var updatedTenant = await context.Tenants.FindAsync(tenantId);
        updatedTenant!.Name.Should().Be("Test 2");
    }
}
