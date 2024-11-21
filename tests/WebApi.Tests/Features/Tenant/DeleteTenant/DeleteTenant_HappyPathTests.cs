using Microsoft.EntityFrameworkCore;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.DeleteTenant;
public class DeleteTenant_HappyPathTests(ApiApplicationFixture fixture) : UsingApi(fixture)
{
    [Fact(DisplayName = "DELETE /tenant/{tenantId} when tenant exists should return 204")]
    public async Task Delete_WhenTenantExists_ShouldReturn204()
    {
        // Given
        var tenantId = Guid.CreateVersion7();
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Id = tenantId, Name = "Tenant name" });
        await context.SaveChangesAsync(CancellationToken.None);


        // When
        var response = await Fixture.Api.DeleteAsync($"tenants/{tenantId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        context = Fixture.CreateDatabaseContext();
        var tenant = await context.Tenants
            .IgnoreQueryFilters()
            .FirstAsync(tenant => tenant.Id == tenantId, CancellationToken.None);

        tenant.IsDeleted.Should().BeTrue();
    }
}
